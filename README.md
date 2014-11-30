# BDDoc - BDD Documentation  
*Writing test cases for BDD, generate your documentation running your tests and keep it tied to your code*

##Intro
The idea to develop **BDDoc** comes from the need to write test cases for BDD using an easy approach and keep the documentation tied to the source code. 
Having either a documentation file or textual DSL may lead to cases where the documentation and source code are out of sync.

BDDoc can be used with *any testing framework* (currently tested with MSTest and NUnit).
A story is implemented as a class whilst the scenarios of the story are implemented as methods.
The only requirements to be satisfied by the story implementation are three:
* **IStory Interface:** *Story has to implement IStory interface*
* **BDDoc Attributes:** *Story and the scenarios have to be decorated using the BDDoc attributes*
* **Code your steps:** *The scenario implementation has to create a BDDoc's scenario instance and to code steps (Given/And/When/Then)*

![ScreenShot](https://github.com/ngallo/BDDoc/blob/master/docs/images/BDDocImg1.png)

##Usage

####BDDoc

######Setup the unit test project
First of all create a new unit test project using the testing framework preferred such as NUnit, MSTest and so on.
Once done the only step left is to add BDDoc.dll to the project's references.

######Create a story
First create a new class for each story (any name convention can be used), and once done, just implement the interface IStory (namespace BDDoc.IStory).
Each story needs to be decorated with BDDoc's attributes. Attributes to be used are listed below:

- **Configuration Attributes**
    - **StoryInfoAttribute:** *Attribute used to configure the persistence of the story. Two properties can be configured:*
        - **GroupName:** *Can be any string and it is used for grouping stories in the documentation. Input text is used as title of the group.*
        - **StoryId:** *It is used to identify the Story as well as used as name of the bddoc file generated.*
- **Documentation Attributes** *(Order property can be used to decide in which the order attributes will presented in documentation)*
    - **StoryAttribute:** *Represents the story description. Its constructor requires in input a textual description of the story (It is used as title in the generated documentation)*
    - **InOrderToAttribute:** *Its constructor requires in input a textual description which will bepart of the documentation*
    - **AsAttribute:** *Its constructor requires in input a textual description which will be part of the documentation*
    - **IWantToAttribute:** *Its constructor requires in input a textual description which will be part of the documentation*

```csharp
    [StoryInfo("NUnit-ReturnsGoToStockStory", GroupName = "Warehouse")]
    [Story("Returns go to stock")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    public class ReturnsGoToStockStory : IStory 
    {
    }
```

BDDoc defines four documentation attributes, however the framework can be extended by creating custom attributes. 
Custom attributes have to inherit BDDocAttribute and implement the IStoryAttrib interface.

```csharp
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomStoryAttribute : BDDocAttribute, IStoryAttrib
    {
        //Constructors

        public CustomStoryAttribute(string text)
            : base(text, 10) { }
    }
```
Once created the custom attribute, it can be used to decorate the story.

```csharp
    [StoryInfo("NUnit-ReturnsGoToStockStory", GroupName = "Warehouse")]
    [Story("Returns go to stock")]
    [InOrderTo("keep track of stock")]
    [AsA("store owner")]
    [IWantTo("add items back to stock when they're returned")]
    [CustomStory("here my text")]
    public class ReturnsGoToStockStory : IStory
    {
    }
```

######Create a Scenario
A new method to the scenario class has to be created for each scenario. Each of them has to be marked with a ScenarioAttribute. The constructor of the ScenarioAttribute requires in input a textual description of the scenario.

**NOTE:** *The scenarios have to be executed by the testing framework, so they have to be marked as test method (for instance using NUnit a Test attribute is required by each scenario method.).*

The scenario code implementation needs to create a new instance of the scenario class by means of the CreateScenario method, and define the steps (When/And/Given/Then) as shown in the code below. The text passed in input to each attribute will be part of the documentation generated.

```csharp
        [Test]
        [Scenario("Refunded items should be returned to stock")]
        public void RefundedItemsReturnedToStockTest()
        {
            var scenario = this.CreateScenario();
            scenario.Given("a customer previously bought a black sweater from me");
            //---------------------------------------------------------------------//
            // Here test code

            scenario.And("I currently have three black sweaters left in stock");
            //---------------------------------------------------------------------//
            // Here test code

            scenario.When("he returns the sweater for a refund");
            //---------------------------------------------------------------------//
            // Here test code

            scenario.Then("I should have four black sweaters in stock");
            //---------------------------------------------------------------------//
            // Here test code

            scenario.Complete();
        }
```

BDDoc defines one scenario attribute, however the framework can be extended by creating custom attributes. 

Custom attributes have to inherit BDDocAttribute and implement the IScenarioAttrib interface.

```csharp
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CustomScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute(string text)
            : base(text, 10) { }
    }
```

Once created the custom attribute, it can be used to decorate the story.

```csharp
        [Test]
        [Scenario("Refunded items should be returned to stock")]
        [CustomScenario("here my text")]
        public void RefundedItemsReturnedToStockTest()
        {
            ...
        }
```

####BDDocGenerator

###### BDDoc files
After running scenari os files with **bddoc** extension will be generated in the execution folder.
Each bddoc file contains the documentation in xml format.

```xml
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<Story version="1.0" groupname="Warehouse" text="Returns go to stock">
  <Items>
    <Item key="InOrderTo" text="keep track of stock" />
    <Item key="AsA" text="store owner" />
    <Item key="IWantTo" text="add items back to stock when they're returned" />
  </Items>
  <Scenario timestamp="29/11/2014 23:32:07" text="Refunded items should be returned to stock">
    <Items />
    <Steps>
      <Step key="Given" text="a customer previously bought a black sweater from me" />
      <Step key="And" text="I currently have three black sweaters left in stock" />
      <Step key="When" text="he returns the sweater for a refund" />
      <Step key="Then" text="I should have four black sweaters in stock" />
    </Steps>
  </Scenario>
</Story>
```

######Use BDDocGenerator to generate the HTML documentation
The utility BDDocGenerator.exe generates the HTML documentation by parsing the bddoc files in the folder passed in input.

- **BDDocGenerator inputs**
    - **-inputdir:** *Directory which contains bddoc files*
    - **-outputdir:** *Directory which is used to generate the HTML version of the documentation*
    - **-projectname:** *Name of the project, which will be presented on the top of each HTML file*

Below an example how to use BDDocGenerator via command line:
```
c:\..> BDDocGenerator.exe -projectname:"My project name" -inputdir:"c:\Teamp\Documentation" -outputdir:"C:\Temp\MyProjectDocumentation"
BDDoc HTML documentation generation started.
BDDoc HTML documentation generation completed.
```

######Using an MSBUild Targets file
The BDDoc sample solution implements an MSBuild targets file which copy all bddoc files generated into the *output-samples\Documentation* folder. Once done it executes the BDDocGenerator utility providing the *output-samples\Documentation\HTML* folder as the  output folder.

Documentation will be available opening the file *output-samples\Documentation\HTML\index.html*, after following steps listed below to compile the BDDoc.samples solution the HTML.

##Source Code
BDDoc has been developed using VisualStudio 2013, c# and .NET Framework 4.0

####Solution
The project contains two Visual Studio Solutions:
- **BDDoc.sln** (src/BDDoc.sln )
    - This solution contains two projects:
        - BDDoc: It is the library to be referenced by Test projects
        - BDDocGenerator: Utility to be used to generate HTML documentation starting from the xml output generated by BDDoc
- **BDDoc.Samples.sln** (samples/BDDoc.Samples.sln)
    - This solution implements a sample project which contain an exemple of BDDoc Test

####BUILD
BDDoc.Samples solution depend by BDDoc, as its projects are referencing the
BDDoc dll which is in the output folder (Output folder is generated by using the MSBuild file named BDDoc.targets).

The project contains a **build.cmd** file which compile the BDDoc solution and generate the application's artifacts (such as output folder etc) by means of the BDDoc.targets file.

######Configure the build.cmd file
The Build.cmd file restores NuGet packages that are referenced by the solution. In order to accomplish it, NuGet has to be installed and the right path typed into the build.cmd file.
Amend the Build.cmd file assigning to the NuGetExe variable the right path of NuGet.exe.

The default value is: 
```
SET NuGetEXE=C:\temp\tools\NuGet.CommandLine.2.8.3\tools\NuGet.exe
```

######Compile
Execute following commands via command line in order to compile both solutions:

*DEBUG CONFIGURATION*
```
REM Compile BDDoc.sln
build

REM Compile BDDoc.Samples.sln
build -samples
```

*RELEASE CONFIGURATION*
```
REM Compile BDDoc.sln
build -r

REM Compile BDDoc.Samples.sln
build -samples -r
```

Two folders will be created once executed the commands listed above:
- **output/libs**: *This folder contains the BDDoc Dll and BDDocGenerator Utility*
- **output-samples/documentation/html**: This folder contains the HTML documentation generated automatically

######Clean
Execute following commands via command line in order to clean both solutions and artifacts created:

*DEBUG Configuration*
```
REM Clean BDDoc.sln
build clean

REM Clean BDDoc.Samples.sln
build -samples clean
```

*RELEASE Configuration*
```
REM Clean BDDoc.sln
build clean

REM Clean BDDoc.Samples.sln
build -samples clean
```

**NOTE**: **Build.sh** *file can be used via Git Bash instead of the build.cmd file.*
```
bash build.sh -r
bash build.sh -samples -r
```
