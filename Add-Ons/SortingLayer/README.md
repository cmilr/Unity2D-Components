Sorting Layer
===

Unity 4.3 added sorting layers and manual sorting orders to all renderers, however only the sprite renderer exposes the values in the inspector. If you're making a 2D game and want to use a text mesh or a standard MeshRenderer, your only option is to adjust sorting layers in code. This folder provides a couple ways of nicely accessing sorting layers on non-sprite objects:


SortingLayerExposed
---

![Readme_SortingLayerExposed.png](https://raw.github.com/nickgravelyn/UnityToolbag/master/SortingLayer/Readme_SortingLayerExposed.png)

This basic component+editor combo lets you change the sorting properties of _any_ renderer simply by putting the SortingLayerExposed component on the same object. The custom editor will provide you with the UI for choosing the sorting layer and sorting layer order for the renderer attached to the object.

SortingLayerAttribute
---

_This attribute/property drawer was adapted from [ChemiKhazi's pull request](https://github.com/nickgravelyn/UnityToolbag/pull/1)_.

If you want to change an object's sorting layer at runtime but want to configure it in the inspector, this is the better option. Using attributes, you can have any regular integer property show up as a sorting layer in the Inspector. Example usage:

    public class SortLayerTest : MonoBehaviour {
        [UnityToolbag.SortingLayer]
        public int sortLayer1;
    }

This will appear in the inspector with a nice drop down for selecting the sorting layer:

![Readme_SortingLayerAttribute.png](https://raw.github.com/nickgravelyn/UnityToolbag/master/SortingLayer/Readme_SortingLayerAttribute.png)

Then you can use that sorting layer to update a renderer at runtime:

    renderer.sortingLayerID = sortLayer1;

This is good, for example, to have an object that starts on one layer (say, the background) but later needs to be moved to the foreground layer.
