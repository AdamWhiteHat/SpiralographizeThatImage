# Spiralographize That Image


This project was inspired by this 17th century print of a wood engraving by Claude Mellan depicting Jesus Christ wearing a crown of thorns, titled *The Face of Christ* or *Sudarium of Saint Veronica*:
![The Face of Christ by Claude Mellan](https://upload.wikimedia.org/wikipedia/commons/c/ca/Claude_Mellan_-_Face_of_Christ_-_WGA14764.jpg)

The thing I found remarkable about the engraving is that the image is depicted using just a single line--a spiral--of varying thickness.

Here we can see the start of this spiraling line which begins in the center--Jesus's nose:
![Up close detail of the engraving](https://upload.wikimedia.org/wikipedia/commons/2/23/Claude_Mellan_-_Face_of_Christ_%28detail%29_-_WGA14765.jpg)

I though this was such an interesting idea; a single spiraling line of varying thickness that depicts an image, and set out to write a program that could create such a line given any arbitrary image.

My strategy to accomplish this consisted primarily of two high-level steps:

1. Draw a spiral. Allow sufficient white space between each successive line to allow the thickness of the line stroke to vary by a few orders, while still being spaced close enough that the finer details of the image can be represented.
2. Scan a supplied input image pixel by pixel, calculate a metric for how light or dark a pixel is, find the segment of line that corresponds to the same x & y position in the image, and vary the line thickness to an ammount proportional to the calculated metric.

Simple in concept, and it turns out the details were relativly straight forward as well, much to my suprise. The effect is acheived in very few lines of code.

After accomplishing this, I experimented with other techniques to create shading instead of varying the line thickness, such as keeping the line thickness the same throughout, and varying the spacing of the lines instead. This worked moderately well, and exist as different classes of which you call the same GetLineSegments method on. 


--


This is a winforms application that prompts for a input image file when ran. It wants a grayscale image. I find a 600x600 image seems to work best, although any image size can be selected.


Examples:


![Screenshot of the application](https://raw.githubusercontent.com/AdamWhiteHat/SpiralographizeThatImage/master/Einstein.png)



![Screenshot of the application](https://raw.githubusercontent.com/AdamWhiteHat/SpiralographizeThatImage/master/SteveMould.png)

