# TA_Test_Colossal
Worktest assignment from Colossal order.


I was asked to 
"Please write a document explaining the method used to achieve the effect with details on the
texture format choices and other relevant information.
Also write about how you would go optimizing the effect further if there was more time given to
complete the task."

First and foremost:

Adressing the requirements:
● There must be at least one material using a custom shader graph shader and a VFX
graph to achieve the dripping animation. Aside from that, the approach is entirely left to
the artist.
--> I did not figure out how or why to use a VFX Graph for this .
Perhaps there is a workflow reason to use it that I'm not aware of?
In my case - I made a set of material functions which can be applied to other materials to make them work by adjusting the lighting parameters based on the presence of a "water mask" on the surface of the object.

● The effect must look convincing.
--> I couldn't really figure out the best way possible to make this work from close distances - I could just upscale the texture sizes, but that felt like a tape solution and not a proper solution.

● The effect must work on any geometry; it is allowed to bake additional vertex information
if desired.
--> I managed to use a cylindrical projection based from the object pivot, so my version of the effect can adapt to surfaces that curve around a little bit.
For a more dynamic projection system, I think the data could be baked through either textures or vertex colours, even. But I had to redo my cylindrical projection because the math wasnt working, and that adjustment took some time because I followed the wrong thought process. 

● The effect must be memory friendly; optional baking and interpolating of texture data is
welcome.
--> I'm not super familiar with how to optimize by texture data interpolation, but I definitely could have baked info to my geometry, I thought it would be better to make a system which didn't require extra workflow from the arts team though.

● The intensity of the rain should be controllable from dry to super wet.
--> There is control over several features of the effect, but I must admit I wasn't able to immediately figure out a super "nice" way to adjust the intensity of the rain effect besides just applying a global offset to everyhing. 
It is argueably my least favorite part of this shader in terms of customization.

-----

Other thoughts and technical breakdown:

The Shader "S_WetObject" uses two SubGraphs: "SG_RainEffectMask" and "SG_RainEffectParams", those two are the abstractions responsible for adjusting colour, specular and smoothness of the surface of a given material.

SG_RainEffectParams is where those values are blended and adjusted between the wet surface properties and and water drops. The main influence comes from the "WetCoating" property, which is the physical equivalence of "how much is the water stuck to this surface", which gets compounded with the Water_Mask property, to adapt to the moving rainfall texture.

Porousness is an extra slider value to adjust how much does the surface get recoloured by being went (damp surfaces darken more if they're more porous).

Smoothness and Specular (or Water_Equivalents) refer to the respective properties - these get lerped based on the values for the water mask and wet coating.

In a very straightforward manner, we simply plug the outputs from this node to their respective counterparts in the material output.

Now for the SG_RainEffectMask Subgraph - far to the left, there are 3 texture samples - they are used to sample the sliding textures of the rainfall by a cylindric projection, done by the subgraph "SG_WorldAlignedCylincricProj" - which just uses world space to calculate a atan2() around the object's geometry - and most of the effect is done from the output of those nodes:

The second cluster of nodes handles the normals and masking out which parts of the effect should be displayed at which "angles" of the geometry (based on normals and dot products to calculate steep vs shallow angles)

There is another Subgraph here, called "SG_RainDropsStatic" - it is a simple usage of worldspace X-Z to map the accumulated droplets of water on top of the geometry. 

The Texture used for this contains normals information on RG, time-offset information on Blue and "droplet cluster size" on Alpha - this set of information is used to produce an animation and retrieve the normals for the droplets of water that will stick to the top of the object.

Everything else that happens now is essentially using the aforementioned masking through dot products to apply the effects at their correct positions.

In total, it took me around 10 hours spread across 3 days to get this done, which is more than I was initially told to use - but I was asked to submit this under less "pressure" than the programmer worktest, so I hope that the extra time taken is acceptable.




