# SecretAPI ![Downloads](https://img.shields.io/github/downloads/Misfiy/SecretAPI/total)
* SecretAPI is a plugin that extends [LabAPI](https://github.com/northwood-studios/LabAPI) by providing extra features to help devs.

# Features
- CollectionExtensions: Extensions to provide utility for collections like lists and arrays.
- RoomExtensions: Extensions to help with room specific tasks, like checking if a room is safe to teleport to.
- HarmonyExtensions: Extensions to provide more utility to Harmony patching, like adding some updated Harmony features which can't be utilised, i.e. patching by category.
- CustomPlayerEffect: Create custom status effects using the base-game system.
- IRegister: Handle auto registering certain plugin features inheriting this interface and then running `IRegister.RegisterAll(Assembly)` in your plugin's initialise method.
- CallOnLoadAttribute: Allows calling static initialize methods on enable via ``CallOnLoadAttribute.Load(Assembly)``
- CustomSetting: Server Specific Settings without the management hassle, control everything for 1 setting in 1 class, including permissions. A better setting system overall.

# Examples
You can find examples inside the `SecretAPI.Examples` folder above, this contains some example settings, patches using categories and some more.

# Support
* For any issues create an [Issue](https://github.com/Misfiy/SecretAPI/issues/new) or contact me on [Discord](https://discord.gg/RYzahv3vfC).