#2D Development

#Project Assets

- **Prefabs**: Reusable game objects (ex: Bullets, enemies, bonuses).

	Prefabs can be seen as a `class` in a programming language, which can be instantiated into game objects. It’s a mold you can duplicate and change at will in the scene or during the game execution.

- **Scenes**: A scene is basically a level or a menu.

	Contrary to the other objects you create in the “Project” pane, scenes are created with the “File” menu. If you want to create a scene, click on the “New Scene” submenu, then do not forget to save it to the “Scenes” folder.

	Scenes need to be manually saved. It’s a classic mistake in Unity to make some changes to a scene and its elements and to forget to save it after. Your version control tool will not see any change until you scene is saved.
	
- **Sounds**: Music and other sounds, basically.

- **Scripts**: All the code goes here. We use this folder as the equivalent of a root folder in a C# project.

- **Sprites**: Sprites are the images of your game. In a 2D project, sprites are textures that can be used by the 2D tools.

- **Resources**: Allows you to load an object or a file inside a script (using the static Resources class).

#Scene

###Objects

- **Empty Objects**: A trick in Unity is to create an empty game object and use it as a “folder” for other game objects. It will simplify your scene hierarchy.

	Make sure they all are at the (0, 0, 0) position so you can track them easily! The position is not important as these empty objects are not using it.
	
- **Collider**: Add Box Collider 2D as a component of a sprite (e.g. Player).

- **Rigidbody**: Rigidbody 2D will tell to the physics engine how to handle the game object. Furthermore, it will also allow collision events to be raised in scripts. To disable gravity, set Gravity Scale to 0.
	
###Props

Also known as props. These elements aren’t used to improve the gameplay but to visually enhance the scene. (e.g. flying platform sprites).

###Camera

“Projection” flag set to “Orthographic” allows the camera to render a 2D game without taking the 3D into account. Keep in mind that even if you are working with 2D objects, Unity is still using its 3D engine to render the scene. The gif above shows this well.

###Scripts

You can define some methods (called “Message” as we are not using C# inheritance system) that Unity will recognize and execute when needed.

Default scripts come with the Start and Update methods. Here is a short list of the most used “Message” functions:

- `Awake()` is called once when the object is created. See it as replacement of a classic constructor method.

- `Start()` is executed after Awake(). The difference is that the Start() method is not called if the script is not enabled (remember the checkbox on a component in the “Inspector”).

- `Update()` is executed for each frame in the main game loop.

- `FixedUpdate()` is called at every fixed framerate frame. You should use this method over Update() when dealing with physics (“RigidBody” and forces).

- `Destroy()` is invoked when the object is destroyed. It’s your last chance to clean or execute some code.


You also have some functions for the collisions :

- `OnCollisionEnter2D(CollisionInfo2D info)` is invoked when another collider is touching this object collider.

- `OnCollisionExit2D(CollisionInfo2D info)` is invoked when another collider is not touching this object collider anymore.

- `OnTriggerEnter2D(Collider2D otherCollider)` is invoked when another collider marked as a “Trigger” is touching this object collider.

- `OnTriggerExit2D(Collider2D otherCollider)` is invoked when another collider marked as a “Trigger” is not touching this object collider anymore.

###Triggers

A trigger collider raises an event when colliding but is not used by the physics simulation.

It means that a shot will pass through an object on touching — there won’t be any “real” interaction at all. Yet, the other collider is going to have its “OnTriggerEnter2D” event raised.

#Parallax Scrolling

The idea is to move the background layers at different speeds (i.e., the farther the layer is, the slower it moves). If done correctly, this gives an illusion of depth.

- First choice: The player and the camera move. The rest is fixed. A no-brainer if you have a Perspective camera. The parallax is obvious: background elements have a higher depth. Thus, they are behind and seems to move slower.

- Second choice: The player and the camera are static. The level is a treadmill. In a standard 2D game in Unity, we use an Orthographic camera. We don’t have depth at render.

>About the camera: remember the “Projection” property of your camera game object. It’s set to `Orthographic` in our game.<br><br>`Perspective` means that the camera is a classic 3D camera, with depth management. `Orthographic` is a camera that renders everything at the same depth. It’s particularly useful for a GUI or a 2D game.

In order to add the parallax scrolling effect to our game, the solution is to mix both choices. We will have two scrollings:

- The player is moving forward along with the camera.

- Background elements are moving at different speeds (in addition to the camera movement).

###Enemies

Want them to wait and be invincible until they spawn.

We position the Poulpies on the scene directly (by dragging the Prefab onto the scene). By default, they are static and invincibles until the camera reaches and activates them.

The script for Parallax will be similar to the  MoveScript - a speed and a direction applied over time.

###Infinite Background Scrolling

We only need to watch the child which is at the left of the infinite layer. When this object goes beyond the camera left edge, we move it to the right of the layer. Indefinitely.

#UI

###Title Screen

It's where the player lands when starting the game.

Create UI Object (Button, Image, Panel, etc.)