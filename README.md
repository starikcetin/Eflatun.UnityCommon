This repository has UPM support. Paste the following into your dependencies (Packages/manifest.json):

    "com.coffee.git-dependency-resolver": "https://github.com/mob-sakai/GitDependencyResolverForUnity.git#1.1.2",
    "com.coffee.upm-git-extension": "https://github.com/mob-sakai/UpmGitExtension.git#0.9.1",
    "com.starikcetin.eflatun.tracking2d": "https://github.com/starikcetin/Eflatun.Tracking2D.git#0.0.3",

----

# Eflatun.UnityCommon #

This library includes common code that are shared across my Unity projects.


Dependencies
---
- [Eflatun.Expansions](https://github.com/starikcetin/Eflatun.Expansions)


Folders
---
- Inspector:      Editor and Inspector.
- Utils:          Utilities, helpers, extensions, etc.


Notes
---
- You may prefer using this library as a subtree (or submodule) in your git repository.
- *Editor* folders are special folders that Unity use to identify editor-only scripts. So all *Editor* folders in this framework are intended for that purpose. Thus, do not use *Editor* as a custom folder name, use *Inspector* instead.


License
---
MIT license. Refer to the [LICENSE](https://github.com/starikcetin/Eflatun.UnityCommon/blob/master/LICENSE) file.

Copyright (c) 2018 S. Tarık Çetin

