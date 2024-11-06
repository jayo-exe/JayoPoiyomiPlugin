# Jayo's Poiyomi Shader Plugin for VNyan

A VNyan Plugin that allows you to control and change the values of animated properties on your Poiyomi shaders through your VNyan node graphs. Adjust colors, textures, vectors, and more!

# Table of contents
1. [Installation](#installation)
2. [Controlling Poiyomi Properties](#controlling-poiyomi-properties)
3. [Usage](#usage)
    1. [Inbound Triggers](#inbound-triggers)
        1. [Set Float Property](#set-float-property)
        2. [Set Int Property](#set-int-property)
        3. [Set Color Property](#set-color-property)
        4. [Set Color Property By Hex](#set-color-property-by-hex)
        5. [Set Vector Property](#set-vector-property)
        6. [Set Texture Property Scaling](#set-texture-property-scaling)
        7. [Set Texture Property Offset](#set-texture-property-offset)
4. [Usage (Legacy)](#usage-legacy)
    1. [Inbound Triggers (Legacy)](#inbound-triggers-legacy)
        1. [Set Float Property (Legacy)](#set-float-property-legacy)
        2. [Set Int Property (Legacy)](#set-int-property-legacy)
        3. [Set Color Property (Legacy)](#set-color-property-legacy)
        4. [Set Color Property By Hex (Legacy)](#set-color-property-by-hex-legacy)
        5. [Set Vector Property (Legacy)](#set-vector-property-legacy)
        6. [Set Texture Property Scaling (Legacy)](#set-texture-property-scaling-legacy)
        7. [Set Texture Property Offset (Legacy)](#set-texture-property-offset-legacy)
5. [Development](#development)
6. [Special Thanks](#special-thanks)

## Installation
1. Grab the ZIP file from the [latest release](https://github.com/jayo-exe/JayoPoiyomiPlugin/releases/latest) of the plugin.
2. Extract the contents of the ZIP file _directly_ into your VNyan installation folder_.  This will add the plugin files to yor VNyan `Items\Assemblies` folders.
3. Launch VNyan, confirm that a button for the plugin now exists in your Plugins window! 

The plugin also includes a sample graph that can demonstrate how to use the plugin to make changes to shader properties.  You can install this to see some practical exaxples and use cases.

## Controlling Poiyomi Properties
In order to control a shader property on a Poiyomi material, it must have been marked as "Animated" or "Renamed When Animated" before the shader was locked.  The plugin UI contains a Properties tab which lets you see and browse all of the Poiyomi-driven materials that are currently loaded, and easily copy them for use it triggers.

Property changes are applied to **every Poiyomi material in the scene**!  To target specific materials, use Poiyomi's built-in "Renamed when animated" feature so that the property on that material is given a unique name.

## Usage

To adjust a property's value, a VNyan Node graph should be used to call a specially-named trigger.  The value sockets on the "Call Trigger" node are used to pass in names and values of properties to change.  You can create complex graphs in VNyan to set properties according to changing conditions!

### Inbound Triggers

The triggers read specific value sockets from the "Call Trigger" node to accept arguments to be used to set shader properties.  `Text 1` will always contain the name of the shader property to be set, `Text2` will always contain info about the value(s) to set, and `Value 1` is always optional, and is used to specify a number of milliseconds to transition to the new value.
`Value 2`, `Value 3` and `Text 3` are unused in any of the triggers in this pluigin.

The triggers for each type of property have different expectations for the value portion passed into `Text 2` based on the property type.  The property name, target valuecan use either literal values, or can pass in `<textParameters>` or `[floatParameters]` in a similar way to how those structures are used in graph nodes.  This makes it easy to write a trigger name once, and control the values it sets by setting parameters before the trigger is called.
For example, you could call the trigger  `_xjp_setcolor` and pass a text value like `[redvalue],[greenvalue],[bluevalue],[alphavalue]` into the `Text 2` slot and it would use the values from those VNyan parameters to determine the color to set.

#### Set Float Property
Trigger Name: `_xjp_setfloat`

Value 1: (optional) a number of milliseconds for which to transition from the old value to the new value
Value 2: _unused_
Value 3: _unused_

Text 1: The name of the Float- or Range-type shader property you wish to set, e.g. `_SomeProperty_MyMaterial`
Text 2: Text representing the float value to which the property should be set, e.g. `420.69`
Text 3: _unused_

Set a Float- or Range-type property matching the provided name to the provided value; optionally changing this gradually over a set amount of time

#### Set Int Property
Trigger Name: `_xjp_setint`

Value 1: (optional) a number of milliseconds for which to transition from the old value to the new value
Value 2: _unused_
Value 3: _unused_

Text 1: The name of the Integer-type shader property you wish to set, e.g. `_SomeProperty_MyMaterial`
Text 2: Text representing the integer value to which the property should be set, e.g. `69420`
Text 3: _unused_

Set an Integer-type property matching the provided name to the provided value; optionally changing this gradually over a set amount of time

#### Set Color Property
Trigger Name: `_xjp_setcolor`

Value 1: (optional) a number of milliseconds for which to transition from the old value to the new value
Value 2: _unused_
Value 3: _unused_

Text 1: The name of the Color-type shader property you wish to set, e.g. `_SomeProperty_MyMaterial`
Text 2: Text representing the color value to which the property should be set, represented as r,g,b or r,g,b,a values. e.g. `0.8,0.6,0.9` or `1.0,0.5,0.25,1.0`
Text 3: _unused_

Set a Color-type property matching the provided name to a Color defined by the provided value *[r]*,*[g]*,*[b]* values and an optional alpha of *[a]* ; optionally changing this gradually over a set amount of time

#### Set Color Property By Hex
Trigger Name: `_xjp_setcolorhex`

Value 1: (optional) a number of milliseconds for which to transition from the old value to the new value
Value 2: _unused_
Value 3: _unused_

Text 1: The name of the Color-type shader property you wish to set, e.g. `_SomeProperty_MyMaterial`
Text 2: Text representing the color value to which the property should be set, represented as an HTML Color string. e.g. `#abc123`, `#abcd1234`, `indigo`, `#eee`
Text 3: _unused_

Set a Color-type property matching the provided name to a Color defined by an HTML Color string in the provided value. e.g. ); optionally changing this gradually over a set amount of time

#### Set Vector Property
Trigger Name: `_xjp_setvector`

Value 1: (optional) a number of milliseconds for which to transition from the old value to the new value
Value 2: _unused_
Value 3: _unused_

Text 1: The name of the Vector-type shader property you wish to set, e.g. `_SomeProperty_MyMaterial`
Text 2: Text representing the Vector values to which the property should be set, represented as a 2-, 3- or 4-part set of coordinates. e.g. `1.2,3.5`, `2.4,6.8,10.4`, `1,2.33,4.9,5.682`, `#eee`
Text 3: _unused_

Set a Vector-type property matching the provided name to a Vector defined by the provided value *[x]*,*[y]*, an optional *[z]* and an optional *[w]* ; optionally changing this gradually over a set amount of time
This allows control over Vector2, Vector3, and Vector4 properties.

#### Set Texture Property Scaling
`_xjp_settexscale;;<propname>;;[x],[y];;[time]`
`_xjp_settexscale;;<propname>;;[x],[y]`

Set the *Texture Scaling* of a Texture-type property matching the provided name to a Vector defined by the provided value *[x]*,*[y]*; optionally changing this gradually over a set amount of time

#### Set Texture Property Offset
`_xjp_settexoffset;;<propname>;;[x],[y];;[time]`
`_xjp_settexoffset;;<propname>;;[x],[y]`

Set the *Texture Offset* of a Texture-type property matching the provided name to a Vector defined by the provided value *[x]*,*[y]*; optionally changing this gradually over a set amount of time


## Usage (Legacy)

The following details the old way of calling this plugin's triggers with specially-struictured trigger names.  This behaviour is included to avoid breaking any curent setups, but should be considered **deprecated** as it will be removed in a later version.

For any new node graph work that interfaces with this plugin, you are encouraged to use the new process detailed above, using the value sockets to pass arguments to the triggers.

### Inbound Triggers (Legacy)

To adjust a property's value, a VNyan Node graph should be used to call a specially-named trigger.  The trigger name includes all of the information that the plugin needs to set the desired property.  You can create complex graphs in VNyan than can dynamically construct these trigger names to set properties according to changing conditions!

The trigger names follow a formula like `_xjp_set{type};;{property};;{value};;{time}`, so calling the trigger `_xjp_setfloat;;_HueShift_Shirt;;0.75;;400` would set a *float*-type property named *_HueShift_Shirt* to a value of *0.75*, transitioning tot his new value over a period of *400*ms.
The {time} portion can also be omitted completely if the change is meant to be instant, like `_xjp_setfloat;;_HueShift_Shirt;;0.75`.

The triggers for each type of property have different expectations for the {value} portion of the trigger name based on the type.  The {property}, {value}, and {time} portions can use either literal values, or can pass in `<textParameters>` or `[floatParameters]` in a similar way to how those structures are used in graph nodes.  This makes it easy to write a trigger name once, and control the values it sets by setting parameters before the trigger is called.
For example, you could call a trigger like `_xjp_setcolor;;<targetProp>;;[redvalue],[greenvalue],[bluevalue],[alphavalue];;[transtime]` and it would use the values from those VNyan parameters to determine the property to change, the color to set, and the transition time for the change.

#### Set Float Property (Legacy)
`_xjp_setfloat;;<propname>;;[value];;[time]`
`_xjp_setfloat;;<propname>;;[value]`

Set a Float- or Range-type property called *<propname>* to a value of *[value]*; optionally over a time period of *[time]* miliseconds

#### Set Int Property (Legacy)
`_xjp_setint;;<propname>;;[value];;[time]`
`_xjp_setint;;<propname>;;[value]`

Set an Int-type property called *<propname>* to a value of *[value]*; optionally over a time period of *[time]* miliseconds

#### Set Color Property (Legacy)
`_xjp_setcolor;;<propname>;;[r],[g],[b],[a];;[time]`
`_xjp_setcolor;;<propname>;;[r],[g],[b],[a]`
`_xjp_setcolor;;<propname>;;[r],[g],[b];;[time]`
`_xjp_setcolor;;<propname>;;[r],[g],[b]`

Set a Color-type property called *<propname>* to a Color defined by *[r]*,*[g]*,*[b]* values and an optional alpha of *[a]* ; optionally over a time period of *[time]* miliseconds

#### Set Color Property By Hex (Legacy)
`_xjp_setint;;<propname>;;<hexcode>;;[time]`
`_xjp_setint;;<propname>;;<hexcode>`

Set a Color-type property called *<propname>* to a Color defined by an HTML Color string *<hexcode>* (e.g. `#abc123`, `#abcd1234`, `indigo`, `#eee`); optionally over a time period of *[time]* miliseconds

#### Set Vector Property (Legacy)
`_xjp_setvector;;<propname>;;[x],[y],[z],[w];;[time]`
`_xjp_setvector;;<propname>;;[x],[y],[z],[w]`
`_xjp_setvector;;<propname>;;[x],[y],[z];;[time]`
`_xjp_setvector;;<propname>;;[x],[y],[z]`
`_xjp_setvector;;<propname>;;[x],[y];;[time]`
`_xjp_setvector;;<propname>;;[x],[y]`

Set a Vector-type property called *<propname>* to a Vector defined by *[x]*,*[y]*, an optional *[z]* and an optional *[w]* ; optionally over a time period of *[time]* miliseconds.
This allows control over Vector2, Vector3, and Vector4 properties.

#### Set Texture Property Scaling (Legacy)
`_xjp_settexscale;;<propname>;;[x],[y];;[time]`
`_xjp_settexscale;;<propname>;;[x],[y]`

Set the *Texture Scaling* of a Texture-type property called *<propname>* to a Vector defined by *[x]*,*[y]*; optionally over a time period of *[time]* miliseconds.

#### Set Texture Property Offset (Legacy)
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
