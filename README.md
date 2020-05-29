# VR360Authoring

This project aims at providing an immersive authoring tool for composing interactive 360-degree videos.

It is being developed using Unity 3D and C#. 

Currently, our tool provides a player for playing interactive 360-degree videos following the model described in the paper "An Authoring Model for Interactive 360 videos" accepted for publication "In Proceedings of ICME 2020 Workshop: Tools for Creating XR Media Experiences, London, UK, 03 2020".

## Cite our work

```bibtex
@inproceedings{2020_07_mendes,
  author={Mendes, Paulo and Guedes, Álan and Moraes, Daniel and De Albuquerque
  Azevedo, Roberto Gerson and Colcher, Sérgio},
  title={An Authoring Model for Interactive 360 videos},
  booktitle={2020 IEEE International Conference on MultimediaExpo Workshops (ICMEW)}, 
  year={2020},
  note={(To be Published)},
  langid={english},
  langidopts={variant=american},
}
```

## Preview Video

The video bellow shows an interactive 360-degree video composed using the model we propose. In this video, a "mirror" of the stage is shown on the bottom of the screen when the user is not looking at the stage. Moreover, it has a spatial audio that gives the user the impression that it is coming from the stage.

[![Watch the video](https://img.youtube.com/vi/BPVGBCFifP0/hqdefault.jpg)](https://www.youtube.com/watch?v=BPVGBCFifP0)

The XML code for this interactive 360-degree video is show bellow.
```
 <presentation360>
 <head>
   <style id="stage" r="0,7" phi="110" theta="−10"/>
 </head>
 <body entry="concert">
   <scene360 id="concert" src="concert.mp4">
     <audio id="mainAudio" begin="0s" src="audio.wav" style="stage"/>
     <hotspot id="hotspotStage" style ="stage" begin="1s" uringNotLookingAt="mirrorStage"/>
     <mirror id="mirrorStage" src="hotspotStage" r="4" phi="0" heta="20" followCamera="true"/>
   </scene360>
 </body>
 </presentation360>
```


## media credits

- <https://www.youtube.com/watch?v=lHqa9gwOGHY>
