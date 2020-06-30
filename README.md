# VR360Authoring

This project aims at providing an immersive authoring tool for composing interactive 360-degree videos.

It is being developed using Unity 3D (2018.3.9f1) and C#. 

Currently, our tool provides a player for playing interactive 360-degree videos following the model described in the paper "An Authoring Model for Interactive 360 videos" accepted for publication "In Proceedings of ICME 2020 Workshop: Tools for Creating XR Media Experiences, London, UK, 03 2020".

## References

Our work is available on [IEEE Xplore](https://ieeexplore.ieee.org/document/9105958) and [Research Gate](https://www.researchgate.net/publication/342097903_AN_AUTHORING_MODEL_FOR_INTERACTIVE_360_VIDEOS).

### Cite our work

```bibtex
@INPROCEEDINGS{9105958,  
author={P. R. C. {Mendes} and Á. L. V. {Guedes} and D. d. S. {Moraes} and R. G. A. {Azevedo} and S. {Colcher}},  
booktitle={2020 IEEE International Conference on Multimedia Expo Workshops (ICMEW)},   
title={An Authoring Model for Interactive 360 Videos},   
year={2020},  
volume={},  number={},  pages={1-6},}
```

## Preview Video

The video bellow shows an interactive 360-degree video composed using the model we propose. In this video, a "mirror" of the stage is shown on the bottom of the screen when the user is not looking at the stage. Moreover, it has a spatial audio that gives the user the impression that it is coming from the stage.

[![Watch the video](https://img.youtube.com/vi/BPVGBCFifP0/hqdefault.jpg)](https://www.youtube.com/watch?v=BPVGBCFifP0)

The XML code for this interactive 360-degree video is shown bellow.
```
 <presentation360>
 <head>
   <style id="stage" r="0,7" phi="110" theta="−10"/>
 </head>
 <body entry="concert">
   <scene360 id="concert" src="concert.mp4">
     <audio id="mainAudio" begin="0s" src="audio.wav" style="stage"/>
     <hotspot id="hotspotStage" style ="stage" begin="1s" duringNotLookingAt="mirrorStage"/>
     <mirror id="mirrorStage" src="hotspotStage" r="4" phi="0" theta="20" followCamera="true"/>
   </scene360>
 </body>
 </presentation360>
```


## media credits

- <https://www.youtube.com/watch?v=lHqa9gwOGHY>
