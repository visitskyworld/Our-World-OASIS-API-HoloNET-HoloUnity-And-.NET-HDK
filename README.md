# OASIS API / Our World / HoloNET Altha v0.0.1

The core OASIS (Open Advanced Sensory Immersion System) API that powers Our World and manages the central profile/avatar/karma system that other satellite apps/games plug into it and share. This also includes HoloNET that allows .NET to talk to Holochain, which is where the profile/avatar is stored on a private decentralised, distributed network. This will be gifted forward to the Holochain community along with the HoloUnity3D SDK/Lib/Asset coming soon... ;-)

The first phase of Our World will be a de-centralised distributed XR Gamified 3D Map replacement for Google Maps along with the Avatar/Profile/Karma & OASIS API system. The satellite apps/games will be able to create their own 2D/3D object to appear on the real-time 3D map.

Next it will implement the ARC (Augmented Reality Computer) Membrane allowing the revolutionary next-gen operating system to fully interface & integrate with the 3D Map & Avatar/Karma system as well as render its own 3D interfaces and 2D HUD overlays on top of the map, etc.

Next, it will port Noomap to Unity and will implement a Synergy Engine allowing people to easily find and match solutions/desires/passions and to also find various solution providers, which again will be fully integrated with the 3D Map & Avatar/Karma system.

## HoloNET

This allows .NET to talk to Holochain, which is where the profile/avatar is stored on a private decentralised, distributed network. This will be gifted forward to the Holochain community If there is demand for HoloNET and people wish to contribute we may consider splitting it out into it's own repo...

This is also how Holochain can talk to Unity because Unity uses C#/.NET as it's backend scripting language/tech.

This will help massively turbo charge the holochain ecosystem by opening it up to the massive .NET and Unity communities and open up many more possibilities of the things that can be built on top of Holochain. You can build almost anything you can imagine with .NET and/or Unity from websites, desktop apps, smartphone apps, services, AAA Games and lots more! They can target every device and platform out there from XBox, PS4, Wii, PC, Linux, Mac, iOS, Android, Windows Phone, iPad, Tablets, SmartTV, VR/AR/XR, MagicLeap, etc

**We are a BIG fan of Holochain and are very passionate about it and see a BIG future for it! We feel this is the gateway to taking Holochain mainstream! ;-)**

### How To Use HoloNET


**NOTE: This documentation is a WIP, it will be completed soon, please bare with us, thank you! :)**


You start by instaniating a new HoloNETClient class found in the `NextGenSoftware.Holochain.HoloNET.Client` project passing in the holochain websocket URI to the constructor as seen below:

````c#
HoloNETClient holoNETClient = new HoloNETClient("ws://localhost:8888");
````

Next, you can subscribe to a number of different events:

````c#
holoNETClient.OnConnected += HoloNETClient_OnConnected;
holoNETClient.OnDataReceived += HoloNETClient_OnDataReceived;
holoNETClient.OnZomeFunctionCallBack += HoloNETClient_OnZomeFunctionCallBack;
holoNETClient.OnGetInstancesCallBack += HoloNETClient_OnGetInstancesCallBack;
holoNETClient.OnSignalsCallBack += HoloNETClient_OnSignalsCallBack;
holoNETClient.OnDisconnected += HoloNETClient_OnDisconnected;
holoNETClient.OnError += HoloNETClient_OnError;
````

Now you can call the `Connect()` method to connect to Holochain.

````c#
await holoNETClient.Connect();
````

Once you received a `OnConnected` event callback you can now call the `GetInstances()` method to get back a list of instances the holochain conductor you connected is currently running.

````c#
if (holoNETClient.State == System.Net.WebSockets.WebSocketState.Open)
{
        await holoNETClient.GetHolochainInstancesAsync();
}
````

Now you can use the instance(s) as a parm to your future Zome calls...

Now you can call one of the `CallZomeFunctionAsync()` overloads:

````c#
await holoNETClient.CallZomeFunctionAsync("1", "test-instance", "our_world_core", "test", ZomeCallback, new { message = new { content = "blah!" } });
````

Please see below for more details on the various overloads available for this call as well as the data you get back from this call and the other methods and events you can use...

#### The Power of .NET Async Methods

You will notice that the above calls have the `await` keyword prefixing them. This is how you call an `async` method in C#. All of HoloNET, HoloOASIS & OASIS API methods are async methods. This simply means that they do not block the calling thread so if this is running on a UI thread it will not freeze the UI. Using the `await` keyword allows you to call an `async` method as if it was a syncronsous one. This means it will not call the next line until the async method has returned. The power of this is that you no longer need to use lots of messy callback functions cluttering up your code as has been the pass with async programming. The code path is also a lot easier to follow and manitain.

Read more here:
https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/

#### Events

You can subscribe to a number of different events:

| Event                  | Desc                                                                                                     |
| ---------------------- | -------------------------------------------------------------------------------------------------------- |
| OnConnected            | Fired when the client has successfully connected to the Holochain conductor.                             |
| OnDisconnected         | Fired when the client disconnected from the Holochain conductor.                                         |
| OnError                | Fired when an error occurs, check the params for the cause of the error.                                 |
| OnGetInstancesCallBack | Fired when the hc conductor has returned the list of hc instances it is currently running.               |
| OnDataReceived         | Fired when any data is received from the hc conductor. This returns the raw JSON data.                   |
| OnZomeFunctionCallBack | Fired when the hc conductor returns the response from a zome function call. This returns the raw JSON    |   |                        | data as well as the actual parsed data returned from the zome function. It also returns the id, instance,|   |                        | zome and zome function that made the call.                                                               |
| OnSignalsCallBack      | Fired when the hc conductor sends signals data. NOTE: This is still waiting for hc to flresh out the     |   |                        | details for how this will work. Currently this returns the raw signals data.                             | 



##### OnZomeFunctionCallBack

````c#
 private static void HoloNETClient_OnZomeFunctionCallBack(object sender, ZomeFunctionCallBackEventArgs e)
        {
            Console.WriteLine(string.Concat("ZomeFunction CallBack: Id: ", e.Id, ", Instance: ", e.Instance, ", Zome: ", e.Zome, ", ZomeFunction: ", e.ZomeFunction, ", Data: ",  e.ZomeReturnData, ", Raw Zome Return Data: ", e.RawZomeReturnData, ", Raw JSON Data: ", e.RawJSONData, ", IsCallSuccessful: ", e.IsCallSuccessful? "true" : "false"));
            Console.WriteLine("");
        }
````

 | Param              | Desc                                               |
 | ------------------ | -------------------------------------------------- |
 | Id                 | The id that made the request.                      |
 | Instance           | The hc instance that made the request.             |
 | Zome               | The zome that made the request.                    |
 | ZomeFunction       | The zome function that made the request.           |
 | ZomeReturnData     | The parsed data that the zome function returned.   |
 | RawZomeReturnData  | The raw JSON data that the zome function returned. |
 | RawJSONData        | The raw JSON data that the hc conductor returned.  |


#### Methods

HoloNETClient contains the following methods:

* `Connect()`
* `CallZomeFunctionAsync()`
* `ClearCache()`
* `Disconnect()`
* `GetHolochainInstancesAsync()`
* `SendMessageAsync()`
* `SendMessageAsync()`

##### CallZomeFunctionAsync

This is the main method you will be using to invoke zome functions on your given zome. It has a number of handy overloads making it easier and more powerful to call your zome functions and manage the returned data.

````c#
public async Task CallZomeFunctionAsync(string id, string instanceId, string zome, string function, ZomeFunctionCallBack callback, object paramsObject, bool matchIdToInstanceZomeFuncInCallback = true, bool cachReturnData = false)
````

| Param                               | Desc                                                                                            
| ----------------------------------- | ---------------------------------------------------------------------------------------------- |
| id                                  | The unique id you wish to assign for this call (NOTE: There is an overload that omits this     |  |                                     | param, use this overload if you wish HoloNET to auto-generate and manage the id's for you).    | 
| instance                            | The instance running on the holochain conductor you wish to target.                            |
| zome                                | The name of the zome you wish to target.                                                       |
| function                            | The name of the zome function you wish to call.                                                |
| delegate                            | A delegate to call once the zome function returns.                                             |
| paramsObject                        | A basic CLR object containing the params the zome function is expecting.                       |
| matchIdToInstanceZomeFuncInCallback | This is an optional param, which defaults to true. Set this to true if you wish HoloNET to give| |                                     | the instance, zome \& zome function that made the call in the callback/event. If this is false | |                                     | then only the id will be given in the |  callback. This uses a small internal cache to match up| |                                     | the id to the given instance/zome/function. Set this to false if you wish to | save a tiny     | |                                     | amount of memory by not utilizing this cache.                                                  |
| cachReturnData                      | This is an optional param, which defaults to false. Set this to true if you wish HoloNET to    | |                                     | cache the JSON response retrieved from holochain. Subsequent calls will return this cached     |  |                                     | data rather than calling the Holochain |conductor again. Use this for static data that is not  |  |                                     | going to change for performance gains.                                                         |

**More to come soon...**

## HoloOASIS

HoloOASIS uses HoloNET to implement a Storage Provider (IOASISStorage) for the OASIS System. It will soon also implement a Network Provider (IOASISNET) for the OASIS System that will leverage Holochain to create it's own private de-centralised distributed network called ONET (as seen on the OASIS Architecture Diagram below).

This is a good example to see how to use HoloNET in a real world game/platform (OASIS/Our World).

## HoloUnity

We will soon be creating a Asset for the Unity Asset Store that will include HoloNET along with Unity wrappers and examples of how to use HoloNET inside Unity.

In the codebase you will find a project called NextGenSoftware.OASIS.API.FrontEnd.Unity, which shows how the ProfileManager found inside the OASIS API Core (NextGenSoftware.OASIS.API.Core) is used. When you instantiate the ProfileManager you inject into a Storage Provider that implements the IOASISStorage interface. Currently the only provider implemented is the HoloOASIS Provider.

The actual Our World Unity code is not currently stored in this repo due to size restrictions but we may consider using GitHub LGE later on. We are also looking at GitLab and other alternatives to see if they allow greater storage capabilities free out of the box (since we are currently working on a very tight budget but you could change that by donating below! ;-) ).

## .NET HDK

We will soon also begin work on the .NET HDK to open up the amazing Holochain to the massive .NET & Unity ecosystem's, which will help turbocharge the holochain ecosystem they are trying to build...

.NET supports compiling to WASM so we know this is possible... ;-)

We are looking for devs who would be interested in this exciting mini-project, so if you are interested please get in touch either on the channel below or by emailing us on ourworld@nextgensoftware.co.uk or david@nextgensoftware.co.uk. We look forward to hearing from you! :)

https://chat.holochain.org/appsup/channels/net-hdk 

https://github.com/NextGenSoftwareUK/Holochain-.NET-HDK

A placeholder has also been added for the work to begin in this repo in the project Holochain.NextGenSoftware.HoloNET.HDK. Just as with NextGenSoftware.Holochain.HoloNET.Client, this project may be split out into its own repo and then linked to this one as a sub-module in future...

We have been tracking a number of different solutions to allow .NET to compile to WASM and the most promising so far is CoreRT (a AOT (Ahead Of Time) Compiler for .NET Core):

https://github.com/dotnet/corert/blob/master/Documentation/how-to-build-WebAssembly.md

This will allow managed C# code to be compiled into any native language including WASM.


## The Power Of Holochain, .NET, Unity & NodeJS Combined!

The front-end is built in Unity, the middle layer is built in C#/.NET and the back-end is built-in Holochain. 

### ARC & Noomap Integration

The middle layer will also soon interface with the amazing ARC (Augmented Reality Computer) operating system being built by my good friend and cosmic brother Chris Lacombe over at S7 Foundation (previously called Noomap). He is also the creator of Noomap, a 3D fractal mind mapping tool that has some communtites very excited! :)

http://noomap.info/
http://iwg.life/s7foundation/

ARC is currently being built in NodeJS and untilises a Semantic Graph to store and represent it's data, it will also contain a revolutionary AI system. We cannot say more on this at the moment because Chris wants to keep this project under the radar at the moment...

### Node.JS Integration

The OASIS Architecture will interface to ARC/Node.JS using Edge.js:

https://github.com/tjanczuk/edge

This will allow both .NET and NodeJS to run in the same process and make cross function calls as if they were native.

We are working very closely with Chris & S7 to fully synergise & intrgrate ARC & Noomap into the OASIS Architecture & Our World.


## ARC, Noomap & IWG (Infinite World Game) Will Be Fully Integrated

ARC, Noomap & IWG will be fully integrated into the OASIS Architecture. The IWG is VERY similar to Our World and has a LOT of overlap and is something we are currenty exporing and synergising but it looks like they will for a start share the same central avatar/profile/karma system that is currently being built in this very repo.


## Turbocharge the Holochain ecosystem!

Because the OASIS Architecture makes use of .NET, Unity, NodeJS & Holochain we have access to 3 massive well established ecosystems along with all their devs & resources. This will massively help turbocharge the holochain ecosystem as well as help raise awareness of it...


## OASIS Architecture Diagram

The Architecture diagram can be found on our website below but it is also in the root of the repo cunningly named OASIS Architecture Diagram.png

![alt text](https://github.com/dellams/OASIS-API-And-HoloNET/blob/master/OASIS%20Arcitecture.png "OASIS Architecture Diagram")

## Open Modular Design

As you can see from the diagram the OASIS architecture is very modular, open and extensible meaning any component can easily be swapped out for another without having to make any changes to the rest of the system. It will use MEF (Managed Extensibility Framework) so the components can even be swapped out without having to re-compile any of the existing code, you simply drop the new component into a hot folder that the system will pick up on the next time you restart.

The components are split into 11 sub-systems/layers:

* Storage (IOASISStorage Interface)
* Network (IOASISNET Interface)
* Renderer (IOASIS2DRenderer & IOASIS3DRenderer Interfaces)
* XR/Eye Tracking
* Haptic Feedback
* Realtime Emotional Feedback System
* Face Recognition
* Motion Tracking
* Input
* OAPP Templates
* OASIS Engine/API

Currently HoloOASIS implements the IOASISStorage interface. In future it will also implement the IOASISNET interface.

**PLEASE MAKE SURE YOU READ THE DESCRIPTION BOXES ON THE DIAGRAM FOR MORE INFO ON HOW THE SYSTEM WILL WORK.**

**NOTE: This is still a WIP, so the above is likely to evolve and change as we progress...**

## Our World/OASIS Will Act As The Bridge For All (Legasy, IPFS, Holochain, Ethereum, SOLID & More!)

As you can see from the architecture diagram, the system will act as the bridge for all platforms and devices due to it being very open and modular by design. In future there will be support for IPFS, Ethereum, SOLID plus many more. This will help users of both legacy apps/games/websites and blockchain slowly migrate to holochain since it will help expose it to them all. The OASIS API will act as a stepping stone as well as help Everything talk to Everything for maximum compatibility.

## Implement Your Own Storage/Network/Renderer Provider

Thanks to the system being very open/modular by design you can easily implement your own Storage/Network/Renderer Provider by simply implementing the IOASISStorage/IOASTNET/IOASIS2DRenderer/IOASIS3DRenderer interfaces respectively. For example you could create a MongoDB, MySQL or SQL Server Storage Provider. This also ensures forward compatibility since if a new storage medium or network protocol comes out in the future you can easily write a new provider for them without having to change any of the existing system. 

The same applies if a new 3D Engine comes out you want to use.

## Switch To A Different Provider In RealTime

The system can even switch to a different Storage/Network Provider in real-time as a fall-over if one storage/network provider goes down for example. It could even use more than one Storage/Network provider since certain providers may be better suited for a given task than another, this way you get the best of both worlds as well as ensure maximum compatibility and uptime.

The same applies for the Renderer Provider, it could use one provider to render 2D and another for 3D, it could even use more than one for for both 2D and/or 3D.

## Our World Overview

Our World is an exciting immersive next generation 3D XR educational game/platform/social network/ecosystem teaching people on how to look after themselves, each other and the planet using the latest cutting-edge technology. It teaches people the importance of real-life face to face connections with human beings and encourages them to get out into nature using Augmented Reality similar to Pokémon Go but on a much more evolved scale. This is our flagship product and is our top priority due to the massive positive impact it will make upon the world...

It is the XR Gamification layer of the new interplanetary operation system, which is being built by the elite technical wizards stationed around the world. This will one day replace the current tech giants such as Google, FaceBook, etc and act as the technical layer of the New Earth, which is birthing now. It is a 5th dimensional and ascension training platform, teaching people vital life lessons as well as acting as a real-time simulation of the real world.

As well as helping to make the world a better place, this game will be pushing the boundaries of what is currently possible with technology. It will feature augmented reality, virtual reality, motion detection, voice recognition and real-time emotional feedback. It will use technology in ways that has not been done before and in areas where it has been done, it will innovate and take it to the next level...

### Open World/New Ecosystm/Asset Store/Internet/Operating System/Social Network

It is much more than just a free open world game where you can build and create anything you can imagine and at the same time be immersed in an epic storyline. it is an entirely new ecosystem/asset store/internet/Operating System/social network, it is the future way we will be interacting with each other and the world through the use of technology. Smaller satellite apps/game will plug into it and share your central profile/avatar where you gain karma for doing good deeds such as helping your local communities, etc and lose karma for being selfish and not helping others since it mirrors the real world where you have free will. There is nothing else out there like this, nothing even comes close, this will change everything... There is a reason we are called NextGen Software! ;-)

Our World is built on top of the de-centralised, distributed nextgen internet known as Holochain.

### First AAA MMO Game To Run On Holochain

Our World will be the first AAA MMO game and 2D/3D Social Network to run on HoloChain and the Blockchain. It will also be the first to integrate a social network with a MMO game/platform as well as all of these technologies and devices together. As with the rest of the game, it will be leading the way in what can be done with this NextGen Technology for the benefit and upliftment of humanity. 

### Smartphone Version

The smartphone version will be a geolocation game featuring Augmented Reality similar to Pokemon Go but on a much more evolved scale (yes, we were designing this long before Pokemon Go came out!).

### Console Version

The console/desktop version will be similar to a Sandbox and MMORPG (Massive Multiplayer Online Role Playing Game) but will be nothing like any other games such as World Of WarCraft & MineCraft. It will in fact define its own genre setting the new bar for others to follow, this truly has never been done before and will take the world by storm! The one thing it will share with them is that it will be a massive open world that billions of players can explore and build together... 

Both versions will share the same online world/multiverse where users logged in through the smartphone versions will be able to interact with the console/desktop versions in real-time within a massive scale persistent Multiverse.

### Virtual Ecommerce

As well as smaller apps/games being allowed to plug into Our World either sharing just the central avatar/profile (data) or full UI integration, content creators/businesses can also create shops (where people can purchase real items in VR that are then delivered to your door so in effect is virtual e-commerce), buildings or even entire zones/lands/worlds. They can rent virtual spaces within the game. Please contact us on ourworld@nextgensoftware.co.uk if you wish to receive special early adopter discounts...

### The Tech Industry Have A Morale & Social Resonsability

The software industry has the power to transform lives through engaging people with innovative products that help them to grow and develop.  Recent popular examples include health apps, mindfulness apps and mind training games.

We wish to take this to the next level and help make the world a better place by using technology for good, by bringing people together and to support, guide and educate everyone on how we can all live happier, fulfilling lives and at the same time how we can help save our planet.

People learn at a young age how to act and behave and this shapes the future generations and the world they will create. Due to the majority of games these days involving violence, sex, gambling, drugs & crime, this is conditioning the youth of today to the sort of world they will create tomorrow.

With the advent of Virtual Reality now making these violent games even more immersive and realistic where the boundaries between games and reality is shrinking by the day, it is imperative we take some social and moral responsibility and start using technology to help create a better world by improving people's life's as well as respecting the environment and planet that sustains us.

Kids today are playing very violent games such as Call Of Duty which are used as brainwashing techniques to desensitise us to violence and also act as a training and recruitment tool for the military (which they have now admitted). The same goes for flight simulators being used to train and recruit drone pilots.

We hope you will agree this is totally unacceptable and is part of why there is so much war, violence, etc in today’s world. It is time we start using technology to teach people the correct life lessons. Our World acts as a simulation for the real world and teaches them how to create a better world in the simulation and then shows how they can then implement these important lessons in real life. Read more on our previous blog post about violence in video games:

https://www.ourworldthegame.com/single-post/2018/03/14/Good-they-are-finally-start-to-take-the-violence-in-video-games-seriously 

Gambling is being forced onto kids more and more in the form of loot boxes where real money is asked for to receive a random prize and now it’s got so bad that money is actually needed to progress within the game. Everything seems to be geared around how much people can be exploited and how much money can be sucked out of them, this is even more wrong for kids. Read more on our previous post about this below:

https://www.ourworldthegame.com/single-post/2017/11/29/Do-you-think-its-right-kids-are-gambling-in-games 

We wish to reach the kids who are glued to their phones and consoles and never go outside, this game will encourage them to get out into parks and interact with people in fun creative ways face to face instead of through their phones.

### Teach Kids The Right Life Lessons

The game will also be teaching people especially kids important vital life lessons and show how they can then implement them in the real world. Part of the way this will be done is by merging the real world with Our World using the latest cutting-edge technology such as Augmented Reality. We wish to get kids and everyone else off their devices and back into nature and to start having real face to face interactions again. We wish to use technology for the upliftment and benefit of humanity and the planet and not just as an escape mechanism or a way to exploit people by selling their data to the highest bidder.

### Remember How Powerful YOU Are!

Our World reminds people how powerful they are and empowers them to be the person they have always wanted to be, to live their life to their FULL potential without any limitations. Everyone has a gift for the world and with Our World we can help them find it. We want to empower people to take responsibility for our beautiful planet, which is currently in crisis and so needs EVERYONE to help make a difference. The entire world is the Our World team, we want everyone to get involved so they can feel they are part of something greater than themselves and at the same time ensure there is a future for our kids and grandkids.

### Bringing People Together

We wish to bring people together, build online communities, encourage people to reach out and help strangers for the greater good of all. To encourage people to come and work together and to show how everyone benefits if they put their differences aside and start all rowing together. It will model the real world and also act as a simulation and training environment for how to make the real world a better place.

### We are Building The Evolved Benevolent Version Of The OASIS

We are building the evolved benevolent version of the OASIS featured in the popular Ready Player One novel and Spielberg film. The OASIS is only about 40% of what Our World is to give you an idea of the sheer size and magnitude of this project! It is aimed at saving the world rather than leading to its destruction due to the neglect it faced when everyone escaped into the OASIS. Ready Player One has proven so popular that Spielberg & Warner Brothers have now released the blockbuster film, which we hope will help promote Our World further. It is about someone with Autism who creates a revolutionary 3D VR Platform which takes the world by storm because it is so far ahead of everything else out there. The creator of the 3D VR platform known as the OASIS grew up in the 80's, is obsessed with the 80's and had guitar lessons as a kid, which also describes our founder David Ellams. 

Read more in our previous post here:

https://www.ourworldthegame.com/single-post/2017/09/08/Our-World-Is-The-Benevolent-Evolved-Sister-of-The-Ready-Player-One-OASIS-VR-Platform 

### Asscension/God Training & Mirror Of Reality Technology

These are the Last Days of Mortal Man through this God Training Programme.

The self-reflective immersive XR game that has been created as ascension technology to help the user to discover their higher self through learning important lessons in how to be, think and feel as a human being. Through “karma” each individual can build themselves to be a better version of themselves that on a sub conscious level will teach them how this can be applied in the real world. The game truly is a mirror for reality. 

Some important points about its potential capabilities and why it could be truly unique:

**Bio Scan technology**- through mapping of brain waves, it can suggest activities and exercises that correlate to the analysis it receives teaching the user to be more mindful about health and wellbeing.

**Life cycle** - There will be time constraints on how long each player can be in Our World for. The vital energy will correlate with reality meaning they will not be burn themselves out locked in the game. Teaching the individual once again the important lessons of having a balanced lifestyle. The character, like the player needs to be looked after.

**Virtual Advertising** - Companies can use the advertising and be awarded prime spots based on their own karma value meaning that those that act more responsibly and consciously in real life (such as giving to charity, being green, looking after their employees, etc) will have access to the most prime spots, as opposed to those who pay the most.

**Time Bending Treasures** - Messages and gifts can be buried as Easter eggs for others to collect at later dates bending the nature of time. 


## Road Map

**Version 1 - Smartphone Platform - The AR version** - Map of present day - In correlation with Time - **IN ACTIVE DEVELOPMENT**. We hope to have an early prototype of this around 2020 Q1/Q2 with more evolved prototypes being released throughout the year. Depending on how many resources/devs we can attract we hope to have a first altha release by 2021, possibly 2022.

**Version 2 - Desktop/Console Platforms - The VR Version** - Game version that starts in Past with a true history of Earth. Not Time Correlated. We hope work on this can begin by 2020 (if additional funds/resources can be secured by then) and will be done in parallel with the Smartphone version. Remember these are not seperate products, and fully integrate with each other where players share the same immersive persistent real-time open world.

**Version 3 - The XR/IR Version** - The XR version that becomes the immersive, self reflective reality that combines both aspects of console and smartphone versions (The OASIS). We hope we will secure MASSIVE funds by 2021/2022 latest so this can begin dev around that time, this is Ready Player One OASIS time with life like graphics and things you can only begin to imagine right now! ;-)

## Next Steps

Not in priority order:

* Add HTTP support to HoloNET.
* Implement IOASISNET interface for HoloOASIS Provider.
* Add built-in HC Conductor to HoloNET so it can fire up it's own conductor without needing to do this manually.
* Add a ZomeProxyGenerator tool so it can auto-generate a C# Zome Proxy that wraps around HoloNET calls (the code would be similar to what is in HoloOASIS)
* Continue with Unity integration and development of HoloUnity, which will then also be gifted forward to the wonderful holochain community... :)
* Refactor HoloNET to split out the websocket JSON RPC 2.0 implementation from the holochain specific logic so the websocket JSON RPC code can be re-used with the OASIS API websocket implementation coming soon...
* Implement OASIS API Websocket JSON RPC 2.0 implementation.
* Implement OASIS API Websocket HTTP implementation.
* Implement OASIS API HTTP Restful WebAPI implementation.
* Finish implementing avatar screen in Unity.
* Place avatar on 3D map using users current location (geolocation) from their device GPS.
* Implement Mapping/Routing API for 3D Map in Unity.
* Implement Places Of Interest (Holons) on 3D Map.
* Implement ARC Membrane API.
* Implement Unity Nlogger.
* Implement animated cars, planes, water, etc on 3D Map.
* Fix bug so when zooming out on 3D Map it shows the full globe instead of going white.
* Implement demo satellite apps/games/websites to show how OASIS API works.
* Implement OASIS API in a number of real apps/games/websites that are waiting and ready...
* Implement Quests on 3D Map (geolocation).
* Implement AR Mode in parks, etc.
* Implement Synergy Engine matching solution providers to requesters.
* Port Noomap to Unity.
* Plus LOTS & LOTS more to come! ;-)

## Donations Welcome! :)

We are working full-time on this project so we have no other income so if you value it, we would really appreciate a donation to our crowd funding page below:

https://www.gofundme.com/f/ourworldthegame

Every
 little helps, even if you can only manage £1 it can still help make all the difference! Thank you! :)

We would really appreciate if you could donate anything you can afford, even if it's just a pound, if everyone did that then we would be able to massively accelerate this very urgent and important project for a world in need right now. I think everyone can justify a pound if it meant saving the world don't you think? 

It's even better to spend a pound on this project rather than buying a lottery ticket since you have more chance of being hit by a car than winning the jackpot, then even by some fluke you did win, there is no point having millions if there is no world left to enjoy it on. 

Think about it...

If you can't afford to contribute, then that's fine, you can still help by getting the word out there!

Our Facebook page is here:

https://www.facebook.com/ourworldthegame 

Please make sure you LIKE it and spread the word and get as many of your friends and family to LIKE it too, many thanks & much appreciated! :)

Every reward above £100 will automatically get your name added to the credits for the app/network which will be seen by billions...

Please ready more on the website:
http://www.ourworldthegame.com

* **What will be your legacy?**

* **Do you want to be in on the ground floor of the upcoming platform that will take the world by storm?
The platform that is going to win many rewards for the ground-breaking work it will do. Do you want to be a hero of your own life story?**

* **Want to tell your kids and grandkids that you helped make it happen and go down in history as a hero?**

* **What kind of world do you want to leave to the next generation?**

* **Want to be part of something greater than yourself?**

* **How can you do your part to create a better world?**

* **This is HOW you do your part...**

* **Be the change you wish to see in the world...**

**NOTE: WE HAVE ONLY DISCLOSED ABOUT 10% OF WHAT OUR WORLD / THE OASIS IS, IF YOU WISH TO GET INVOLVED OR INVEST THEN WE WILL BE HAPPY TO SHARE MORE, PLEASE GET IN TOUCH, WE LOOK FORWARD TO HEARING FROM YOU...**

**TOGETHER WE CAN CREATE A BETTER WORLD.**

## Devs/Contributions Welcome! :)

We would love to have some much needed dev resource on this vital project not only for Holochain but also for the world so if you are interested please contact us on either ourworld@nextgensoftware.co.uk or david@nextgensoftware.co.uk. Thank you, we look forward to hearing from you! :)

## Other Ways To Get Involved

If you cannot code or donate, then no problem, you can help in other ways! :) You can share our website/posts, give us valuable feedback on our site, etc as well as submit ideas for Our World. We are also looking for people to join for every department/area such as PR, Sales, Support, Admin, Accounting, Management, Strategy, Operations, etc  

So if you feel you want to help or get involved please contact us on ourworld@nextgensoftware.co.uk, we would love to hear from you! :)

You can also get involved on our forum here:

http://www.ourworldthegame.com/forum

## Websites

Read more on this project on our websites below:

http://www.ourworldthegame.com

http://www.nextgensoftware.co.uk
