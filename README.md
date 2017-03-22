# AutoMapperPostBuilder
Automatically validate your AutoMapper mapping after each build
### Introduction
If you write mapping or translation code for setting properties from one object to another, you have probably used [AutoMapper](http://automapper.org) by [Jimmy Bogard](https://github.com/jbogard). AutoMapper is a simple little library built to solve a deceptively complex problem. One downside is, often you will find out during runtime that you missed one or more properties in a mapping. This project will let you have feedback directly after compile whether or not your mapping is correct. You don't want to wait for the runtime exceptions (well, actually you will, but......just read on).

### Basics
AutoMapper is not a silver bullet. You can find a lot of good posts or rants about why one should not use AutoMapper. Even Jimmy Bogard himself describes scenarios in which you might better not use AutoMapper. Nevertheless, in the projects that I encounter that rely heavily on AutoMapper, I implement the AutoMapperPostBuilder.
The build in functionality of AutoMapper `Mapper.AssertConfigurationIsValid()` is used directly after compilation. Just by calling an Invoke in the postbuild event of your project in which you are using AutoMapper, it will give you insight what properties you missed in your mapping. 

### Getting started
Reference the AutoMapperPostBuilder project and reference it with the option 'Copy Local = True'. This will ensure that the AutoMapperPostBuilder.exe is copied to your project output folder. In the properties of your project, fill in the 'Post-build event command line':
```csharp
    "$(TargetDir)AutoMapperPostBuilder.exe" "$(TargetPath)" AutoMapperPostBuilder_Sample.MappingClass Invoke
```
Where in this example the class to be invoked is the AutoMapperPostBuilder_Sample.MappingClass. Just replace that name with the class name you want to invoke.
The AutoMapperPostBuilder.exe takes the following arguments:
* full path of your project output (eg. bin/debug/YourAssembly.dll)
* the namespace and class name of your mapping class
* the `Invoke` command

The class you want to invoke needs to have the following method:
```csharp
    public static void Invoke() {}
```
In the (static) constructor of the class with , where you probably have your standard mapping all written out, you need to end with:
```csharp
    Mapper.AssertConfigurationIsValid();
```

A simple mapping class could look just like this:
```csharp
    using AutoMapper;
    
    namespace AutoMapperPostBuilder_Sample
    {
        static class MappingClass
        {
            static MappingClass()
            {            
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<SampleClassB_1, SampleClassA_1>()                                        
                        .ForMember(dest => dest.InnerProp1, opt => opt.MapFrom(src => src.InnerProp10))
                        .ForMember(dest => dest.InnerProp2, opt => opt.MapFrom(src => src.InnerProp20));
                });
                
                Mapper.AssertConfigurationIsValid();
            }
    
            public static void Invoke() {}        
        }
    }
```

### The result
Now, if you compile your project and the build succeeds (initially), the post-build event will fire the AutoMapperPostBuilder.exe that will look for the specified class and invokes it. This will build your mapping and will call AssertConfigurationIsValid. If any of your mappings is faulty, it will display it in your build-output-window. 
Even better, the post-build event will notice that AutoMapperPostBuilder.exe has exited with code 1. This will result in a fail build. And that's exactly what you want, because you do not want to wait for the exception by starting your application and just finally hit that code.


### Sample project
The project `AutoMapperPostBuilder_Sample`, which is included in the AutoMapperPostBuilder solution shows just how easy it is to implement. The current code compiles just fine, and therefor you will not see much in your Build-output window. Just comment out the mapping of a certain property, and you'll see the hint that AutoMapper will give you.
