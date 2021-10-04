1. Create a ASP.Net Core Web Application Name OraWebApiCore: Select Template as Model-View-Controller
2. Add the required references to work with Oracle Server Database & .Net Framework:
	a. Go to Tools Menu-> Nuget Package Manager-> Package Manager Consol
	b. Run the command: Install-Package Oracle.ManagedDataAccess.Core
	Or Install "Oracle.ManagedDataAccess.Core" From Manage NuGet Packages...
	And Rebuild Project
3. Add Models, Interface and Services Folder in the application
4. Configure the database and add connection string to do the following steps:
	a) Run below scripts into the Sql Developer:
		Create User APICoreDB Identified By APICoreDB;
		Grant Dba To APICoreDB; Commit;
		Create Table APICoreDB.Student(Id Number,Name Varchar2(50),Email Varchar2(50));
		Create Unique Index APICoreDB.Pk_Stud On APICoreDB.Student (Id);
		Alter Table APICoreDB.Student Add (Constraint Pk_Stud Primary Key (Id) Using Index APICoreDB.Pk_Stud);
		  
		Insert Into APICoreDB.Student(Id,Name,Email) Values(1,'Sumon','smsbd9@gmail.com');
		Insert Into APICoreDB.Student(Id,Name,Email) Values(2,'Mahbub','smsbd21@gmail.com');
		Select * From APICoreDB.Student;
	
	b) Now add below scripts into the appsettings.json file:
		"ConnectionStrings": {
		"OracleDbConnection": "User Id=APICoreDB; Password=APICoreDB; data source=127.0.0.1:1521/ORCL;"
		//"OracleDbConnection": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));User Id=YourDBase;Password=YourPwd"
	  },
5. Add Specific Model Classes into the Models Folder:
	public class [Class Name]
	{
		public int Id {get; set;}
		public string Name {get; set;}
		public string Remarks {get; set;}
	}
6. Add [Service Name as NameService] Class inside Services Folder
7. Add [Interface Name as INameService] Interface inside Interface Folder and Make it public
8. Now Inherit INameService Interface on NameService Class inside Services Folder
9. Open Startup.cs file & add services inside the ConfigureServices() Method:
	//services.AddRazorPages();
	services.AddTransient<INameService, NameService>();
	services.AddSingleton<IConfiguration>(Configuration);
	services.AddMvc().AddRazorPagesOptions(options =>
	{
		options.Conventions.AddPageRoute{"/Home/Index"};
		//options.Conventions.AddPageRoute{"/NameController/Index"};
	});
10. Add Empty MVC Controller [Controller Name as NameController] inside Controller Folder
11. Add some method inside IStudentService Interface:
		IEnumerable<Student> GetStudents();
        Student GetStudentById(int id);
        Student DeleteStudent(int id);
        Student AddStudent(Student emp);
        Student EditStudent(Student emp);
12. Now add below code inside StudentService class
	private readonly string connectionString;
	public StudentService(IConfiguration _config)
    {
        connectionString = _config;
    }
	
13. Add below code inside StudentController Controller:
	private readonly IStudentService studentService;
    public StudentController(IStudentService _studentService)
    {
        studentService = _studentService;
    }
