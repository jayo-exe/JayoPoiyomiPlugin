# Jayo's Poiyomi Shader Plugin for VNyan

A VNyan Plugin that allows you to control and change the values of animated properties on your Poiyomi shaders through your VNyan node graphs. Adjust colors, textures, vectors, and more!

# Table of contents
1. [Installation](#installation)
2. [Usage](#usage)
    1. [Controlling Poiyomi Properties](#controlling-poiyomi-properties)
    2. [Inbound Triggers](#inbound-triggers)
        1. [Set Float Property](#set-float-property)
        2. [Set Int Property](#set-int-property)
        3. [Set Color Property](#set-color-property)
        4. [Set Color Property By Hex](#set-color-property-by-hex)
        5. [Set Vector Property](#set-vector-property)
        6. [Set Texture Property Scaling](#set-texture-property-scaling)
        7. [Set Texture Property Offset](#set-texture-property-offset)
3. [Development](#development)
4. [Special Thanks](#special-thanks)

## Installation
1. Grab the ZIP file from the [latest release](https://github.com/jayo-exe/JayoPoiyomiPlugin/releases/latest) of the plugin.
2. Extract the contents of the ZIP file _directly_ into your VNyan installation folder_.  This will add the plugin files to yor VNyan `Items\Assemblies` folders.
3. Launch VNyan, confirm that a button for the plugin now exists in your Plugins window! 

The plugin also includes a sample graph that can demonstrate how to use the plugin to make changes to shader properties.  You can install this to see some practical exaxples and use cases.

## Usage
### Controlling Poiyomi Properties
In order to control a shader property on a Poiyomi material, it must have been marked as "Animated" or "Renamed When Animated" before the shader was locked.  The plugin UI contains a Properties tab which lets you see and browse all of the Poiyomi-driven materials that are currently loaded, and easily copy them for use it triggers.

To adjust a property's value, a VNyan Node graph should be used to call a specially-named trigger.  The trigger name includes all of the information that the plugin needs to set the desired property.  You can create complex graphs in VNyan than can dynamically construct these trigger names to set properties according to changing conditions!

Property changes are applied to **every Poiyomi material in the scene**!  To target specific materials, use Poiyomi's built-in "Renamed when animated" feature so that the property on that material is given a unique name.

### Inbound Triggers

The trigger names follow a formula like `_xjp_set{type};;{property};;{value};;{time}`, so calling the trigger `_xjp_setfloat;;_HueShift_Shirt;;0.75;;400` would set a *float*-type property named *_HueShift_Shirt* to a value of *0.75*, transitioning tot his new value over a period of *400*ms.
The {time} portion can also be omitted completely if the change is meant to be instant, like `_xjp_setfloat;;_HueShift_Shirt;;0.75`.

The triggers for each type of property have different expectations for the {value} portion of the trigger name based on the type.  The {property}, {value}, and {time} portions can use either literal values, or can pass in `<textParameters>` or `[floatParameters]` in a similar way to how those structures are used in graph nodes.  This makes it easy to write a trigger name once, and control the values it sets by setting parameters before the trigger is called.
For example, you could call a trigger like `_xjp_setcolor;;<targetProp>;;[redvalue],[greenvalue],[bluevalue],[alphavalue];;[transtime]` and it would use the values from those VNyan parameters to determine the property to change, the color to set, and the transition time for the change.

#### Set Float Property
`_xjp_setfloat;;<propname>;;[value];;[time]`
`_xjp_setfloat;;<propname>;;[value]`

Set a Float- or Range-type property called *<propname>* to a value of *[value]*; optionally over a time period of *[time]* miliseconds

#### Set Int Property
`_xjp_setint;;<propname>;;[value];;[time]`
`_xjp_setint;;<propname>;;[value]`

Set an Int-type property called *<propname>* to a value of *[value]*; optionally over a time period of *[time]* miliseconds

#### Set Color Property
`_xjp_setcolor;;<propname>;;[r],[g],[b],[a];;[time]`
`_xjp_setcolor;;<propname>;;[r],[g],[b],[a]`
`_xjp_setcolor;;<propname>;;[r],[g],[b];;[time]`
`_xjp_setcolor;;<propname>;;[r],[g],[b]`

Set a Color-type property called *<propname>* to a Color defined by *[r]*,*[g]*,*[b]* values and an optional alpha of *[a]* ; optionally over a time period of *[time]* miliseconds

#### Set Color Property By Hex
`_xjp_setint;;<propname>;;<hexcode>;;[time]`
`_xjp_setint;;<propname>;;<hexcode>`

Set a Color-type property called *<propname>* to a Color defined by an HTML Color string *<hexcode>* (e.g. `#abc123`, `#abcd1234`, `indigo`, `#eee`); optionally over a time period of *[time]* miliseconds

#### Set Vector Property
`_xjp_setvector;;<propname>;;[x],[y],[z],[w];;[time]`
`_xjp_setvector;;<propname>;;[x],[y],[z],[w]`
`_xjp_setvector;;<propname>;;[x],[y],[z];;[time]`
`_xjp_setvector;;<propname>;;[x],[y],[z]`
`_xjp_setvector;;<propname>;;[x],[y];;[time]`
`_xjp_setvector;;<propname>;;[x],[y]`

Set a Vector-type property called *<propname>* to a Vector defined by *[x]*,*[y]*, an optional *[z]* and an optional *[w]* ; optionally over a time period of *[time]* miliseconds.
This allows control over Vector2, Vector3, and Vector4 properties.

#### Set Texture Property Scaling
`_xjp_settexscale;;<propname>;;[x],[y];;[time]`
`_xjp_settexscale;;<propname>;;[x],[y]`

Set the *Texture Scaling* of a Texture-type property called *<propname>* to a Vector defined by *[x]*,*[y]*; optionally over a time period of *[time]* miliseconds.

#### Set Texture Property Offset
`_xjp_settexoffset;;<propname>;;[x],[y];;[time]`
`_xjp_settexoffset;;<propname>;;[x],[y]`

Set the *Texture Offset* of a Texture-type property called *<propname>* to a Vector defined by *[x]*,*[y]*; optionally over a time period of *[time]* miliseconds.

## Development
(Almost) Everything you'll need to develop a fork of this plugin (or some other plugin based on this one)!  The main VS project contains all of the code for the plugin DLL, and the `dist` folder contains a `unitypackage` that can be dragged into a project to build and modify the UI and export the modified Custom Object.

Per VNyan's requirements, this plugin is built under **Unity 2020.3.40f1** , so you'll need to develop on this version to maintain compatability with VNyan.
You'll also need the [VNyan SDK](https://suvidriel.itch.io/vnyan) imported into your project for it to function properly.
Your Visual C# project will need to mave the paths to all dependencies updated to match their locations on your machine.  Most should point to Unity Engine libraries for the correct Engine version **2020.3.40f1**.

## Special Thanks
Suvidriel for building and maintaining VNyan (and answering my endless questions)!

Poiyomi and all of the contributors to the fanstastic Poiyomi Shaders!

2.0, Astral Lovelace, LumKitty, and The Last Seahorse for providing the instrumental testing and feedback that lead to most of the features and flexibility that this plugin is able to offer!
