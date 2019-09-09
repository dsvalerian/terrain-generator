# terrain-generator
A Unity tool that procedurally generates a chunk of terrain from Perlin noise heightmaps.

## Live Demo
A live interactive demo of the tool can be found on [my portfolio website](http://dmitrisalov.pythonanywhere.com/terrain-generator).
Users may alter different values in the generation algorithm and the chunk will be updated in real time. To regenerate a brand new chunk, click the "generate" button.

## How It Works
The basis for the terrain generation algorithm is [Perlin noise](https://en.wikipedia.org/wiki/Perlin_noise#targetText=Perlin%20noise%20is%20a%20type,1985%20called%20An%20image%20Synthesizer.) It is a type of noise in which values will interpolate with those surrounding them. This provides a smooth transition from one area of the noise to another. A 2D area of Perlin noise is selected at random, where each value represents the height of a vertex in the 3D terrain mesh. This section is known as a height map.

There is a downside to using heightmaps, which is that they do not allow for generation of caves or any other terrain structures beyond a surface.

## Variables
There are multiple variables that are used to affect the area of Perlin noise that is selected. These variables are as follows:
  * The __noise scale__ is how much space one value of noise takes up in the heightmap. A larger noise scale results in less area of Perlin noise being used to generate the terrain.
  * The __number of octaves__ is how many different layers, or octaves, of Perlin noise will be overlayed to generate the terrain. Different octaves represent different levels of detail in height values, such as mountains or small rocks.
  * The __octave frequency increase__ level is how much more dense each octave gets. This is useful for representing more detail in a smaller area of terrain. For example, there are many more small rocks in a piece of terrain than mountains, so their frequency is increased.
  * The __octave amplitude decrease__ level is how much less height an octave adds to the final terrain, which is important when dealing with smaller objects. For example, small rocks would affect the height of the terrain much less than a mountain would, so their amplitude is decreased.
  * The __mesh height scale__ is a general multiplier for how tall the terrain should be.
  * The __level of detail__ is a multiplier for how many vertices are used in the 3D terrain mesh. This is useful for when many chunks of terrain are being displayed on screen at once, where the level of detail of distant terrain chunks may be reduced at the cost of detail. The loss of detail is usually worth the performance benefits because it is difficult to distinguish the detail of objects at a large distance. To put into perspective how much performance can be increased by reducing the level of detail: at the highest level there are roughly 115,000 polygons in the generated mesh. At the lowest level, there are around only 1,400, a massive improvement.
