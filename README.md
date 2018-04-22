# yaSingleton

Yet Another Singleton library for [Unity](http://www.unity3d.com/). Based on ScriptableObject instead of the more conventionally used MonoBehaviour.

Example Unity project can be found [here](https://github.com/jedybg/yaSingleton-Example-Project). 

## Why

Why use yaSingleton and why use ScriptableObjects?

 * No duplicate instances of your singletons across different scenes.
 * Single point of entry that's guaranteed to run before any of your Awake scripts.
 * Easy versioning. Since singletons are saved as ScriptableObjects, they're outside the scenes and thus easy to version control.
 * Thread-safe LazySingleton.
 * Better performance; all singletons share the same Update events.
 * Easy to integrate with your existing code base.

## Getting Started

Adding yaSingleton to your project can be as easy as downloading it and extracting it anywhere in your **Assets Folder**. Alternatively, for easier updates (and extra points), you can add it as a *git submodule*.

### Adding yaSingleton as a git submodule

If you're not familiar with git submodules, you can get started [here](https://git-scm.com/book/en/v2/Git-Tools-Submodules).

#### Adding the submodule 

I'll describe a simple setup in which yaSingleton lives in the root directory of the project - modify it to suit your needs. 

First, go to your project's directory and run:

```
cd Assets
git submodule add https://github.com/jedybg/yaSingleton.git
```

This will clone the project in a the yaSingleton directory. You'll need to commit the folder and the .gitmodules file (tracking all submodules) to your repository.

#### Updating the submodule

The great thing about submodules is that they're simple repositories. You can easily go to your project's directory and use *git pull* to update to the latest version.

```
cd Assets/yaSingleton
git pull # or any other git command
```

Just remember that this will generate a changeset in *your repository* which you'll have to later commit. 

## Migration to yaSingleton

Migrating to yaSingleton is quite simple.

First, inherit the base class:

```
# public class UIManager : MonoBehaviour becomes
public class UIManager : Singleton<UIManager> 
```

Or if you want the singleton to be lazy-loaded:

```
public class UIManager : LazySingleton<UIManager>
```

Add the CreateAssetMenuAttribute:
```
[CreateAssetMenu(fileName = "UI Manager", menuName = "Singletons/UIManager")]
public class UIManager : Singleton<UIManager> 
```

Next, replace initialization methods:

```
void Awake() {
    // Initialization code
}
void Start() {
    // Initialization code
}
// become:

protected override void Initialize() {
    base.Initialize();
    
    // Initialization code
}
```

Replace deinitialization methods:

```
void OnApplicationQuit() {
    // Deinitialization code
}
void OnDestroy() {
    // Deinitialization code
}
// become:

protected override void Deinitialize() {
    base.Deinitialize();
    
    // Deinitialization code
}
```

Finally, replace Unity event functions:

```
void Update() {
    // Update code
}
void LateUpdate() {
    // Late Update code
}
void FixedUpdate() {
    // Fixed Update code
}
// become:

protected override void OnUpdate() {
    // Update code
}
protected override void OnLateUpdate() {
    // Late Update code
}
protected override void OnFixedUpdate() {
    // Fixed Update code
}
```

All supported Unity events are:
**OnFixedUpdate**, **OnUpdate**, **OnLateUpdate**, **OnApplicationFocus**, **OnApplicationPause**, **OnApplicationQuit**, **OnDrawGizmos**, **OnPostRender**, **OnPreCull**, **OnPreRender**. They're all using the original event's name with the *On* prefix (if it wasn't in the name already).

### Accessing scene objects

Having your singletons in ScriptableObjects means you won't be able to reference scene objects in your inspector fields. Although a hassle at first, this can make your scene architecture a lot cleaner, since it will remove potential dependencies between scenes and the problems associated with them.

Here are three ways you can cleanly access scene objects that happen to work perfectly with yaSingleton.

#### 1. Registration

Instead of your Singleton knowing about the specific objects, you can have the objects register themselves in the singleton. Works great if you're managing many instances of the same behavior (units, items, etc).

```
class MyBehaviour : MonoBehaviour {
    void OnEnable() {
        MySingleton.AddBehaviour(this);     
    }
    void OnDisable() {
        MySingleton.RemoveBehaviour(this);
    }
}
``` 

#### 2. Events

Instead of calling a method on all of the dependant objects, you can have an event to which the dependant objects can hook to.

```
class MySingleton : Singleton<MySingleton> {
    public event Action sampleEvent = () => { };
    
    public void CallEvent() {
        sampleEvent();
    }
}

class MyBehaviour : MonoBehaviour {
    void OnEnable() {
        MySingleton.sampleEvent += OnSampleEvent;  
    }
    void OnDisable() {
        MySingleton.sampleEvent -= OnSampleEvent;  
    }
    void OnSampleEvent() {
        // Awesome code
    }
}
```  

#### 3. Finding objects

Finally and most straightforward, you can simply find the objects in the scene and go about your business. I suggest using [FindGameObjectsWithTag](https://docs.unity3d.com/ScriptReference/GameObject.FindGameObjectsWithTag.html).

```
class MySingleton : Singleton<MySingleton> {
    private GameObject[] _units;
    
    protected override void Initialize() {
        base.Initialize();
        _units = GameObject.FindGameObjectsWithTag("Unit");
    }
}
```  

## Contributing

The project is, of course, open to contributions. Just fork it and post a pull request.

## Authors

* **Jordan Georgiev** - *The one to blame* - [jedybg](https://github.com/jedybg)

## License

This project is licensed under the BSD 3-Clause - see the [LICENSE.md](LICENSE.md) file for details.
