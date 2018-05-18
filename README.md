# starikcetin.UnityCommon #

This library includes common code that are shared across all of my Unity projects.


Folders
---
- Expansions:     Independent of Unity.
- Inspector:      Unity dependent, Editor and Inspector.
- Utils:          Unity dependent.

*If a type uses anything that Unity provides, than it is Unity dependent.*


Notes
---
- You may prefer using this library as a subtree (or submodule) in your git repository.
- *Editor* folders are special folders that Unity use to identify editor-only scripts. So all *Editor* folders in this framework are intended for that purpose. Thus, do not use *Editor* as a custom folder name, use *Inspector* instead.


License
---
MIT license. Refer to the [LICENSE](https://github.com/starikcetin/Unity-CSharp-Common-Library/blob/master/LICENSE) file.

Copyright (c) 2018 S. Tarık Çetin

