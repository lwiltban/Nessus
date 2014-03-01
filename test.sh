#!/bin/bash
# Default hostname and port variables
HOSTNAME=localhost
PORT=28763

# This will parse out the json response for use with the login 
function jsonval {
    temp=`echo $json | sed 's/\\\\\//\//g' | sed 's/[{}]//g' | awk -v k="text" '{n=split($0,a,","); for (i=1; i<=n; i++) print a[i]}' | sed 's/\"\:\"/\|/g' | sed 's/[\,]/ /g' | sed 's/\"//g' | grep -w $prop`
    echo ${temp##*|}
}

echo -e "START: Nessus back end assessment test\n"

echo -e "\n\nTest 1.a. login\n"
json=`curl -u test:password http://$HOSTNAME:$PORT/api/login`
prop='token'
token=`jsonval`
# token is the Nessus token

echo -e "\n\nTest 1.b. retrieve configurations\n"
curl -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/configurations

echo -e "\n\nTest 1.c Let an authenticated user insert and delete configurations"
echo -e "Insert new configuration\n"
curl -sL -w "%{http_code} Insert host3\n" -H "Authorization: NESSUS $token" -X POST -H "Content-Type: application/json" -d "{ 'name' : 'host3','hostname' : 'nessus-xyz.lab.com', 'port' : 1234,'username' : 'shirley'}" http://$HOSTNAME:$PORT/api/configurations

echo -e "\n\nlist configurations to see new configuration\n"
curl -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/configurations

echo -e "\n\nDelete added configuration\n"
curl -sL -w "%{http_code}\n" -H "Authorization: NESSUS $token" -X DELETE http://$HOSTNAME:$PORT/api/configurations/host3

echo -e "\n\nList configurations again, should see host1-2\n"
curl -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/configurations

echo -e "\n\nTest 2. Add/Modify existing routes to list configurations with more than 100,000 entries"
echo -e "Insert new configurations\n"
curl -sL -w "%{http_code} Insert host3\n" -H "Authorization: NESSUS $token" -X POST -H "Content-Type: application/json" -d "{ 'name' : 'host3','hostname' : 'nessus-abc.lab.com', 'port' : 1234,'username' : 'user1'}" http://$HOSTNAME:$PORT/api/configurations
curl -sL -w "%{http_code} Insert host4\n" -H "Authorization: NESSUS $token" -X POST -H "Content-Type: application/json" -d "{ 'name' : 'host4','hostname' : 'nessus-def.lab.com', 'port' : 1243,'username' : 'user1'}" http://$HOSTNAME:$PORT/api/configurations
curl -sL -w "%{http_code} Insert host5\n" -H "Authorization: NESSUS $token" -X POST -H "Content-Type: application/json" -d "{ 'name' : 'host5','hostname' : 'nessus-5.lab.com', 'port' : 1244,'username' : 'user2'}" http://$HOSTNAME:$PORT/api/configurations
curl -sL -w "%{http_code} Insert host6\n" -H "Authorization: NESSUS $token" -X POST -H "Content-Type: application/json" -d "{ 'name' : 'host6','hostname' : 'nessus-same.lab.com', 'port' : 1244,'username' : 'user2'}" http://$HOSTNAME:$PORT/api/configurations
curl -sL -w "%{http_code} Insert host7\n" -H "Authorization: NESSUS $token" -X POST -H "Content-Type: application/json" -d "{ 'name' : 'host7','hostname' : 'nessus-same.lab.com', 'port' : 1244,'username' : 'user2'}" http://$HOSTNAME:$PORT/api/configurations

echo -e "\n\nDo the paging on the get configurations"
echo -e "\nShould see host2-host7\n"
curl -H "Authorization: NESSUS $token" "http://$HOSTNAME:$PORT/api/configurations?max=10000&offset=1"
echo -e "\n\nShould see host5-host6"
curl -H "Authorization: NESSUS $token" "http://$HOSTNAME:$PORT/api/configurations?max=2&offset=4"

echo -e "\n\nTest 3. Add/Modify existing routes to sort the listing by name, hostname, port and/or username"

echo -e "\n\nSort by name\n"
curl -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/configurations?sort=name

echo -e "\n\nSort by hostname,port\n"
curl -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/configurations?sort=hostname,port

echo -e "\n\nTest 1.a logout\n"
curl -sL -w "\n%{http_code}\n" -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/logout

echo -e "\n\nNow logged out, should see 403 on authorized operations\n"
curl -sL -w "%{http_code}\n" -H "Authorization: NESSUS $token" http://$HOSTNAME:$PORT/api/configurations

echo -e "\nDONE: Nessus back end assessment test"
