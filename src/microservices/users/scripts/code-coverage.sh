echo '---------------------------------------------Run unit tests---------------------------------------------'
dotnet test ../tests/UnitTests //p:CollectCoverage=true //p:CoverletOutput=TestResults\\ //p:CoverletOutputFormat=cobertura

echo ''
echo '---------------------------------------------Install reportgenerator tool---------------------------------------------'
dotnet tool install -g dotnet-reportgenerator-globaltool

echo ''
echo '---------------------------------------------Publish coverage reports---------------------------------------------'
reportgenerator "-reports:..\tests\UnitTests\TestResults\coverage.cobertura.xml" "-targetdir:..\tests\UnitTests\TestResults\Coverage\Reports" -reportTypes:htmlInline;