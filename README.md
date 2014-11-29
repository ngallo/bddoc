# BDDoc - BDD Documentation  
Writing test cases for BDD, generate your documentation running your tests and keep it tied to your code

##Intro
The idea to develop BDDoc comes from the need to write tests for BDD using an easy approach and keep the documentation tied to the source code. 
Having either a documentation file or textual DSL may lead to scenario where the documentation is out of sync with the source code.

BDDoc can be used with any testing framework (Currently tested with MSTest and NUnit). Using BDDoc a story is implemented as a class whilst the scenarios of the story are implemented as methods. 
The only requirements to be satisfied by the story implementation are three:
* IStory Interface: *Story has to implement IStory interface*
* BDDoc Attributes: *Story and the scenario methods have to be decorated using the BDDoc attributes*
* Code your steps: *The scenario implementation has to create a BDDoc's scenario instance and to code steps (Given/And/When/Then)*

![ScreenShot](https://github.com/ngallo/BDDoc/blob/master/docs/images/BDDocImg1.png)

##Usage

####Create a BDDoc Test
..

######BDDoc Test Sample
..

####Generate documentation using BDDocGenerator
..

#######BDDocGenerator Sample
..


##Source Code
BDDoc has been developed using VisualStudio 2013, c# and .NET Framework 4.0

####Solution
..

####BUILD
..

