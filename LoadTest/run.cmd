For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c_%%a_%%b)
For /f "tokens=1-2 delims=/:" %%a in ("%TIME: =0%") do (set mytime=%%a%%b)
echo started at V%mydate%_%mytime%
S:\Developer\apache-jmeter-5.3\bin\jmeter -n -t ApiTestPlan.jmx -l CSVReport\ApiLoadTestReport_V%mydate%_%mytime%.csv -e -o HtmlReport\ApiLoadTestReport_V%mydate%_%mytime%
pause
pause