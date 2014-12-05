# BDDoc - BDD Documentation  
*Write test cases for BDD, generate the documentation running your tests and keep it tied to your code*

##Intro
The idea of developing **BDDoc** comes from the need to write test cases for BDD using an easy approach and keep the documentation and the source code closely tied.
Having either a documentation file or a textual DSL may lead to cases where the documentation and source code are out of sync.

BDDoc can be used with *any testing framework* (currently tested with MSTest and NUnit).
A story is implemented as a class whilst the scenarios of the story are implemented as methods.
The only requirements to be satisfied by the story implementation are three:
* **IStory Interface:** *The story has to implement IStory interface*
* **BDDoc Attributes:** *The story and the scenarios have to be decorated using the BDDoc attributes*
* **Code the steps:** *The scenario implementation has to create a BDDoc's scenario instance and coding the steps (Given/And/When/Then)*

Here is an example of generated documentation:
* **Index**: [www.ngallo.it/bddoc/html](http://www.ngallo.it/bddoc/html/index.html)
* **Story**: [www.ngallo.it/bddoc/html/NUnit-ReturnsGoToStockStory.html](http://www.ngallo.it/bddoc/html/NUnit-ReturnsGoToStockStory.html)

![ScreenShot](https://github.com/ngallo/BDDoc/blob/master/docs/images/BDDocImg1.png)

##NuGet Packages
* **BDDoc**: [www.nuget.org/packages/BDDoc/](https://www.nuget.org/packages/BDDoc/)
* **BDDocGenerator**: [www.nuget.org/packages/BDDocGenerator/](https://www.nuget.org/packages/BDDocGenerator/)

##Usage

####BDDoc

######Unit test project setup
First of all create a new unit test project using the testing framework preferred such as NUnit, MSTest and so on. 
Once done the only step to be done is to add BDDoc.dll to the project's references

######Create a story
First create a new class for each story (any name convention can be used), and once done, just implement the interface IStory (namespace BDDoc.IStory).
Each story needs to be decorated with BDDoc's attributes. Attributes to be used are listed below:

- **Configuration Attributes**
    - **StoryInfoAttribute:** *Attribute used to configure the persistence of the story. Two properties can be configured:*
        - **GroupName:** *Can be any string and it is used for grouping stories in the documentation. Input text is used as title of the group.*
        - **StoryId:** *It is used to identify the Story as well as used as name of the bddoc file generated.*
- **Documentation Attributes** *(Order property can be used to decide in which order the attributes will be presented by the documentation)*
    - **StoryAttribute:** *Represents the story description. Its constructor requires in input a textual description of the story (It is used as title by the generated documentation)*
    - **InOrderToAttribute:** *Its constructor requires in input a textual description which will be part of the documentation*
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
Custom attributes have to inherit the BDDocAttribute class and implement the IStoryAttrib interface.

```csharp
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
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
For each scenario a new method has to be added to the Story class. Each of them has to be marked with a ScenarioAttribute. The constructor of the ScenarioAttribute requires in input a textual description of the scenario.

**NOTE:** *The scenarios have to be ran by the testing framework. In order to do this they have to be marked as test method (for instance using NUnit the Test attribute is required by each scenario method).*

The code implementation of the scenario needs to create a new instance of the scenario class by means of the CreateScenario method, and also define the steps (When/And/Given/Then) as shown in the code below. The text passed in input to each attribute will be part of the documentation generated.

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

Custom attributes have to inherit the BDDocAttribute class and implement the IScenarioAttrib interface.

```csharp
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomScenarioAttribute : BDDocAttribute, IScenarioAttrib
    {
        //Constructors

        public CustomScenarioAttribute(string text)
            : base(text, 10) { }
    }
```

Once created the custom attribute, it can be used to decorate the scenario.

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
Files with `bddoc extension` will be generated in the execution folder after running the scenarios. 
Each bddoc file contains the documentation in an xml format.

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
The utility BDDocGenerator.exe generates the HTML documentation by parsing the bddoc files in the directory passed in input.

Usage of the `BDDocGenerator.exe` is as follows:

```text
-inputdir               Directory which contains bddoc files
-outputdir              Directory which is used to save the HTML documentation

Options:
-projectname:           Name of the project, which will be presented on the top of each HTML file
```

Below an example how to use BDDocGenerator via command line:
```batch
cmd> BDDocGenerator.exe -projectname:"My project name" -inputdir:"c:\Temp\BDDocFiles" -outputdir:"C:\Temp\MyProjectDocumentation"
BDDoc HTML documentation generation started.
BDDoc HTML documentation generation completed.
```

######Using an MSBUild Targets file
The BDDoc sample solution implements an MSBuild targets file which copy all bddoc files generated into the *output-samples\Documentation* directory. Once done it executes the BDDocGenerator utility providing the *output-samples\Documentation\HTML* directory as the  output directory.

Following steps listed below to compile the BDDoc.samples solution the HTML Documentation will be available opening the file *output-samples\Documentation\HTML\index.html*

##Source Code
BDDoc has been developed using `VisualStudio 2013`, `c#` and `.NET Framework 4.0`

####Solution
The project contains two Visual Studio Solutions:
- **BDDoc.sln** (src/BDDoc.sln )
    - *This solution contains two projects:*
        - **BDDoc:** *It is the library to be referenced by the unit test projects*
        - **BDDocGenerator:** *Utility to be used to generate the HTML documentation using the xml output generated by BDDoc*
- **BDDoc.Samples.sln** (samples/BDDoc.Samples.sln)
    - *This solution implements a sample project where an exemple of BDDoc Test can be found*

####BUILD
BDDoc.Samples solution depend by BDDoc, as its projects are referencing the BDDoc dll which is in the output folder (Output folder is generated by using the MSBuild file named BDDoc.targets).

The project contains a **build.cmd** file which by means of the BDDoc.targets file first compile the BDDoc solution and then generates the application's artifacts (such as output folder etc).

######Configure the build.cmd file
The Build.cmd file restores NuGet packages that are referenced by the
solution. In order to accomplish it, NuGet has to be installed and the path set into the build.cmd file.

**Amend the Build.cmd file by setting the right path of the NuGet.exe file.**

By default value set is: 
```
SET NuGetEXE=C:\temp\tools\NuGet.CommandLine.2.8.3\tools\NuGet.exe
```

######Compile
Execute following commands via command line in order to compile both solutions:

*DEBUG CONFIGURATION*
```batch
#Compile BDDoc.sln
cmd> build

#Compile BDDoc.Samples.sln
cmd> build -samples
```

*RELEASE CONFIGURATION*
```batch
#Compile BDDoc.sln
cmd> build -r

#Compile BDDoc.Samples.sln
cmd> build -samples -r
```

Two folders will be created once executed the commands listed above:
- **output/libs**: *This folder contains the BDDoc.dll and BDDocGenerator.exe Utility*
- **output-samples/documentation/html**: This folder contains the HTML documentation generated automatically

######Clean
Execute following commands via command line in order to clean both solutions as well as artifacts created:

*DEBUG Configuration*
```batch
#Clean BDDoc.sln
cmd> build clean

#Clean BDDoc.Samples.sln
cmd> build -samples clean
```

*RELEASE Configuration*
```batch
#Clean BDDoc.sln
cmd> build clean

#Clean BDDoc.Samples.sln
cmd> build -samples clean
```

**NOTE**: *When using* **Git bash**, *the* **Build.sh** *file has to be used instead of the build.cmd file.*
```batch
gitbash> bash build.sh -r
gitbash> bash build.sh -samples -r
```
