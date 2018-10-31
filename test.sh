
if ! dotnet build;
then
	echo 
	echo "Build failed"
	exit 1
fi

if ! dotnet test HoneyDo.Test/HoneyDo.Test.csproj;
then
	echo 
	echo "Tests failed"
	exit 1
fi

echo 
echo 
echo "--------------------------------------------"
echo "Build and Test completed"
echo "--------------------------------------------"
