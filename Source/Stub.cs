using System.Security.Permissions;

//THIS IS REQUIRED CODE FOR THE STUBBED ASSEMBLY-CSHARP TO WORK.
//This is currently required for Loader's Thunder Slam
//
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

//This allows us to use the SystemInitializer attribute
//the attribute will call whatever method it's attached to once ror2 starts loading.
//We can add dependencies to the SystemInitializer attribute, an example would be run
//a piece of code that gets automatically ran once the ItemCatalog is initialized
[assembly: HG.Reflection.SearchableAttribute.OptIn]