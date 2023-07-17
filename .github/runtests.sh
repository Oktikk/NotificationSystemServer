#!/bin/bash

FAILURES=0
TEST_PROJECTS=$(find test -name "NotificationSystemServer.Tests.*")

for PROJECT in $TEST_PROJECTS
do 
    (dotnet test ./$PROJECT --no-build --verbosity normal)
    if test "$?" != "0" 
    then ((FAILURES+=1)) 
    fi    
done

exit $FAILURES