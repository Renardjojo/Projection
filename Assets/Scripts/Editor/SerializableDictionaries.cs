using UnityEditor;

[CustomPropertyDrawer(typeof(CommandKeyCodeDict))]
public class CommandKeyCodeDictEditor : SerializableDictionaryPropertyDrawer
{}


[CustomPropertyDrawer(typeof(CommandGamepadCodeDict))]
public class CommandGamepadCodeDictEditor : SerializableDictionaryPropertyDrawer
{ }