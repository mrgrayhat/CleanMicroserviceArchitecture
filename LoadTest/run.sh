currentDate=`date +%s`
echo started at `date` with unique name $currentDate :
jmeter -n -t ApiTestPlan.jmx -l CSVReport\ApiLoadTestReport_$currentDate.csv -e -o HtmlReport\ApiLoadTestReport_$currentDate

