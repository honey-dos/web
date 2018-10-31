
if ! dotnet build;
then
	echo 
	echo "Build failed"
	exit 1
fi

if ! dotnet test HoneyDo.Test/HoneyDo.Test.csproj;
then
	echo 
	echo "xunit Tests failed"
	exit 1
fi

if ! npm run test:ci --prefix HoneyDo.Web/ClientApp;
then
	echo 
	echo "jest Tests failed"
	exit 1
fi

echo 
echo 
echo "--------------------------------------------"
echo "Build and Test completed"
echo "--------------------------------------------"
