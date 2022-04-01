// See https://aka.ms/new-console-template for more information

Console.WriteLine("Enter 1 to pack Linux-run");
//Console.WriteLine("Enter 2 to pack Linux-deb"); To Do
int mode = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("Enter your directory...");
string directory = Console.ReadLine();
bool complete = mode switch
{
    1 => Package.Linux_run.Builder.Build(directory),
    _ => false
};
Console.WriteLine(complete ? "Finish" : "Fail");