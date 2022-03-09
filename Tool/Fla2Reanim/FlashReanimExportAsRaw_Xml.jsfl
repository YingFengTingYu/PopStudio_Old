var gDoc = fl.getDocumentDOM();

function assert(check, message)
{
	if (!check)
	{
		throw "assert: " + message;
	}
}

var gGlobalTime = 0;

function DebugTime(message)
{
	var currentTime = new Date();
	var nowTime = currentTime.getSeconds();
	nowTime += currentTime.getMinutes() * 60;
	nowTime += currentTime.getHours() * 60 * 60;
	
	if (gGlobalTime == 0)
	{
		fl.trace("start time - " + message)
	}
	else
	{
		var delta = nowTime - gGlobalTime;
		fl.trace(delta + " - " + message);	
	}
	
	gGlobalTime = nowTime;	
}

function RadToDeg(rad)
{
	return rad * 57.2957795;
}

function XmlEncode(text)
{
	// must to amp first because we otherwise it will encode the existing replacements
	text = text.replace(/&/g, '&amp;');
	text = text.replace(/</g, '&lt;');
	text = text.replace(/>/g, '&gt;');
	return text;
}

function StringFromFloat(value, maxDecimalPlaces)
{
	var roundValue = value + (5 / Math.pow(10, maxDecimalPlaces + 1));
	
	var str = roundValue.toString();

	var decimalIndex = str.indexOf('.');

	if (decimalIndex == -1)
	{
		return str;
	}

	if (maxDecimalPlaces == 0)
	{
		return str.substring(0, decimalIndex);
	}

	return str.substring(0, decimalIndex + maxDecimalPlaces + 1);
}

function StringSetChar(str, ch, index)
{
	assert(index >= 0 && index < str.length, "index >= 0 && index < str.length");

	return str.substring(0, index) + ch + str.substring(index + 1);
}

function StringReplaceChar(str, lookfor, replacewith)
{
	assert(lookfor.length == 1, "lookfor.legth == 1");
	assert(lookfor.length == 1, "lookfor.legth == 1");
	
	for (i = 0; i < str.length; i++)
	{
		if (str.charAt(i) == lookfor)
		{
			str = StringSetChar(str, replacewith, i)
		}
	}
	
	return str;
}

function MakeURI()
{
	var lastIndex = gDoc.path.lastIndexOf('.');
	
	assert(lastIndex != -1, "lastIndex != -1");
	
	var filename = gDoc.path.substring(0, lastIndex) + ".reanim";

	var colonIndex = filename.indexOf(':');

	assert(colonIndex != -1, "colonIndex != -1");
	
	var filenameNoColon = StringSetChar(filename, '|', colonIndex);
	
	var filenameOtherSlashes = StringReplaceChar(filenameNoColon, "\\", "/" );
	
	return "file:///" + filenameOtherSlashes;
}

function FrameState()
{
	this.prevX = 0;
	this.prevX = 0;
	this.prevY = 0;
	this.prevKX = 0;
	this.prevKY = 0;
	this.prevSX = 1;
	this.prevSY = 1;
	this.prevF = 0;
	this.prevA = 1;
	this.prevImageName = "";
	this.prevFontName = "";
	this.prevText = "";
}

function FindBaseElementAndMatrix(element, elementIndex)
{
	this.baseElement = element;
	this.matrixFinal = this.baseElement.matrix;
	this.numElements = 1;
	this.baseSymbol = this.baseElement;

	while (this.baseElement.elementType == "instance")
	{
		if (this.baseElement.libraryItem.itemType != "movie clip" &&
			this.baseElement.libraryItem.itemType != "graphic")
		{
			break;
		}
	
		numElementsHere = this.baseElement.libraryItem.timeline.layers[0].frames[0].elements.length;
	
		if (numElementsHere == 0)
		{
			break;
		}

		useIndex = 0;

		if (numElementsHere > 1)
		{
			if (elementIndex < numElementsHere && this.numElements == 1)
			{
				useIndex = elementIndex;
				this.numElements = numElementsHere;
			}
			else
			{	
				fl.trace("!!Warning ignoring symbol extra elements in symbol " + this.baseElement.libraryItem.name);
			}
		}
		
		this.baseSymbol = this.baseElement;
		
		// Note that I only handle one symbols with multiple elements
		this.baseElement = this.baseElement.libraryItem.timeline.layers[0].frames[0].elements[useIndex];
		this.matrixFinal = fl.Math.concatMatrix(this.baseElement.matrix, this.matrixFinal);
	}
	
	return this;
}

function WriteReanimFrame(sampleFrame, frameArray, prev, URI, elementIndex)
{
	var frame = frameArray[sampleFrame];
	var elementArray = frame.elements;
	
	if (elementArray.length != 1)
	{
		// encode a blank frame
		if (prev.prevF != -1)
		{
			FLfile.write(URI, "<t><f>-1</f></t>\n", "append");
			prev.prevF = -1;
		}
		else
		{
			FLfile.write(URI, "<t></t>\n", "append");
		}
		
		return 1;
	}	

	var element = elementArray[0];
	var BaseElementAndMatrix = FindBaseElementAndMatrix(element, elementIndex);
	var baseElement = BaseElementAndMatrix.baseElement;
	var matrixFinal = BaseElementAndMatrix.matrixFinal;
	var numElements = BaseElementAndMatrix.numElements;

	var skewX = Math.atan2(matrixFinal.b, matrixFinal.a);
	var skewY = Math.atan2(matrixFinal.c, matrixFinal.d);

	var quarterPi = 0.785398163;
	
	var sx;
	var sy;
	if (Math.abs(skewX) < quarterPi || Math.abs(skewX) > quarterPi * 3)
	{
		sx = matrixFinal.a / Math.cos(skewX);
	}
	else // need to switch way to get scale when cos() is near 0
	{
		sx = matrixFinal.b / Math.sin(skewX);
	}
	if (Math.abs(skewY) < quarterPi || Math.abs(skewY) > quarterPi * 3)
	{
		sy = matrixFinal.d / Math.cos(skewY);
	}
	else // need to switch way to get scale when cos() is near 0
	{
		sy = matrixFinal.c / Math.sin(skewY);
	}

	/*
	fl.trace("a " + matrixFinal.a + " b " + matrixFinal.b + " c " + matrixFinal.c + " d " + matrixFinal.d);			
	fl.trace("sx1 " + matrixFinal.a / Math.cos(skewX) + " sy1 " + matrixFinal.d / Math.cos(skewY));
	fl.trace("sx2 " + matrixFinal.b / Math.sin(skewX) + " sy2 " + matrixFinal.c / Math.sin(skewY));
	*/
	
	var degreesKX = RadToDeg(skewX);
	var degreesKY = -RadToDeg(skewY);
	
	if (sampleFrame > 0)
	{
		// We need to unwrap the rotation if you rotate over/under one full circle
		while (prev.prevKX - degreesKX > 180)
		{
			degreesKX += 360;
		}
		while (prev.prevKX - degreesKX < -180)
		{
			degreesKX -= 360;
		}
		while (prev.prevKY - degreesKY > 180)
		{
			degreesKY += 360;
		}
		while (prev.prevKY - degreesKY < -180)
		{
			degreesKY -= 360;
		}
	} 
	else
	{
		// keep it positive
		if (degreesKX < 0)
		{
			degreesKX += 360;
		}
		if (degreesKY < 0)
		{
			degreesKY += 360;
		}			
	}

	var f = 0;
	
	/* This is unused, and I think it was a lame way to do this, and now it makes NAN floats which is bad
	if (element.filters)
	{
		// Since there isn't a good place to encode the anim frame, we just jam in it a the brightness.
		var filter = element.filters[0];
		f = Math.floor(filter.brightness);
	}
	*/
	
	var a = 1.0;

	if (element.colorAlphaPercent != undefined)
	{
		a = element.colorAlphaPercent / 100;
	}
				
	var imageName = "";
	var fontName = "";
	var stringText = "";
				
	if (element.layer.name == '_ground' || 
	    element.layer.name == 'fullscreen' || 
	    element.layer.name.substring(0,7) == 'locator')
	{
		// these nodes don't have an image
		imageName = "";
	}
	else if (element.layer.name.substring(0,10) == 'attacher__')
	{
		// this is a special attacher nodes for movies
		if (BaseElementAndMatrix.baseSymbol.libraryItem != undefined && BaseElementAndMatrix.baseSymbol.libraryItem.name.indexOf('__') != -1)
		{
		    // use the parent symbol name if it looks ok
		    stringText = BaseElementAndMatrix.baseSymbol.libraryItem.name;
		}
		else if (element.libraryItem != undefined && element.libraryItem.name.indexOf('__') != -1)
		{
		    // use the root symbol name if it looks ok
		    stringText = element.libraryItem.name;
		}
		else if (baseElement.libraryItem != undefined)
		{
		    // or use the bitmap name
			stringText = baseElement.libraryItem.name.replace(/\..*/g, '');
			
			if (stringText.substring(0,10) != 'attacher__')
			{
			    // this is an regular image even though it's in an attacher
			    imageName = "IMAGE_REANIM_" + stringText.toUpperCase();
			    stringText = "";
			}
		}
	}
	else if (baseElement.elementType == "text")
	{
		var tRuns = baseElement.textRuns;
		assert(tRuns.length == 1, "Text should only have one run");
		//fl.trace("text=" + tRuns[0].characters + ", x=" + baseElement.x + ", width=" + baseElement.width);
		//fl.trace("font " + tRuns[0].textAttrs.face);
		//fl.trace("fontsize " + tRuns[0].textAttrs.size);
		//fl.trace("fontoverride " + tRuns[0].textAttrs.url);
		
		var fontOverride = tRuns[0].textAttrs.url;
		
		var fontName = tRuns[0].textAttrs.face.toUpperCase() + Math.round(tRuns[0].textAttrs.size * 0.8);
		
		if (fontOverride != "")
		{
			fontName = fontOverride;
		}

		//fl.trace("fontusing " + fontName);
		
		//FLfile.write(URI, "<font>FONT_" + fontName + "</font>\n", "append");
		//FLfile.write(URI, "<text>" + XmlEncode(tRuns[0].characters) + "</text>\n", "append");
		fontName = "FONT_" + fontName
		stringText = tRuns[0].characters
		
		// This seems to center the text
		matrixFinal.tx -= baseElement.x;
		
		//fl.trace(frameArray[0].elements[0].libraryItem.name);
	}
	else
	{
		if (baseElement.libraryItem == undefined)
		{
			// Use the symbol name if the base element is complicated
			imageName = "IMAGE_REANIM_" + frameArray[0].elements[0].libraryItem.name.toUpperCase();
		}
		else if (baseElement.libraryItem.timeline != undefined &&
				baseElement.libraryItem.timeline.layers[0].frames[0].elements.length == 0)
		{
			// locator nodes don't have an image
			imageName = "";
		}
		else
		{
			imageName = "IMAGE_REANIM_" + baseElement.libraryItem.name.replace(/\..*/g, '').toUpperCase();
		}
	}
			
	//fl.trace("image " + imageName + " frame " + sampleFrame + " s " + frame.startFrame + " d " + frame.duration + " c " + layer.frameCount);

	FLfile.write(URI, "<t>", "append");

	stringX = StringFromFloat(matrixFinal.tx, 1);
	stringY = StringFromFloat(matrixFinal.ty, 1);
	stringDegreesKX = StringFromFloat(degreesKX, 1);
	stringDegreesKY = StringFromFloat(degreesKY, 1);
	stringSX = StringFromFloat(sx, 3);
	stringSY = StringFromFloat(sy, 3);
	stringF = StringFromFloat(f, 0);
	stringA = StringFromFloat(a, 2);
	
	if (prev.prevX != stringX)
	{
		FLfile.write(URI, "<x>" + stringX + "</x>", "append");
	}
	
	if (prev.prevY != stringY)
	{
		FLfile.write(URI, "<y>" + stringY + "</y>", "append");
	}
							
	if (prev.prevKX != stringDegreesKX)
	{
		FLfile.write(URI, "<kx>" + stringDegreesKX + "</kx>", "append");
	}
	
	if (prev.prevKY != stringDegreesKY)
	{
		FLfile.write(URI, "<ky>" + stringDegreesKY + "</ky>", "append");
	}
				
	if (prev.prevSX != stringSX)
	{
		FLfile.write(URI, "<sx>" + stringSX + "</sx>", "append");
	}
	
	if (prev.prevSY != stringSY)
	{
		FLfile.write(URI, "<sy>" + stringSY + "</sy>", "append");
	}
	
	if (prev.prevF != stringF)
	{
		FLfile.write(URI, "<f>" + stringF + "</f>", "append");
	}
	
	if (prev.prevA != stringA)
	{
		FLfile.write(URI, "<a>" + stringA + "</a>", "append");
	}

	if (prev.prevImageName != imageName)
	{
		FLfile.write(URI, "<i>" + imageName + "</i>", "append");
	}
	 
	if (prev.prevFontName != fontName)
	{
		FLfile.write(URI, "<font>" + fontName + "</font>", "append");
	}
	 
	if (prev.prevText != stringText)
	{
		if (stringText == "")
		{
			// underscore means blank string because the empty string means repeat previous
			FLfile.write(URI, "<text>_</text>", "append");
		}
		else
		{
			FLfile.write(URI, "<text>" + XmlEncode(stringText) + "</text>", "append");
		}			
	}
	 
	FLfile.write(URI, "</t>\n", "append");

	prev.prevX = stringX;
	prev.prevY = stringY;
	prev.prevKX = stringDegreesKX;
	prev.prevKY = stringDegreesKY;
	prev.prevSX = stringSX;
	prev.prevSY = stringSY;
	prev.prevF = stringF;
	prev.prevA = stringA;
	prev.prevImageName = imageName;
	prev.prevFontName = fontName;
	prev.prevText = stringText;
	
	//fl.trace("element " + layer.name + " frame " + sampleFrame + " a " + matrixFinal.a + " b " + matrixFinal.b + " c " + matrixFinal.c + " d " + matrixFinal.d);
	//fl.trace("element " + layer.name + " frame " + sampleFrame + " left" + element.left + " top " + element.top + " width " + element.width + " height " + element.height);
	
	return numElements;
}

function WriteReanimLayer(layerIndex, URI)
{	
	var layer = gTimeLine.layers[layerIndex];
	
	if (layer.name[0] == '_' && layer.name != '_ground')
	{
		// These layers aren't exported
		fl.trace("skipping layer " + layer.name);
		return;
	}

	var frameArray = layer.frames;
	
	maxElements = 1;
	
	for (elementIndex = 0; elementIndex < maxElements; elementIndex++)
	{
		layerName = layer.name;
		
		if (elementIndex > 0)
		{
			layerName += elementIndex + 1;
		}
		
		if (!FLfile.write(URI, "<track>\n", "append"))
		{
			assert(false, "can't write to file " + URI);
		}
		
		fl.trace("layer " + layerName);
		FLfile.write(URI, "<name>" + layerName + "</name>\n", "append");		
	
		var prev = new FrameState();
		
		for (sampleFrame = 0; sampleFrame < gNumFrames; sampleFrame++)
		{	
			numElements = WriteReanimFrame(sampleFrame, frameArray, prev, URI, elementIndex);
			if (numElements > maxElements)
			{
				maxElements = numElements;
			}
		}
		
		FLfile.write(URI, "</track>\n", "append");
	}
}

function WriteReanimFile()
{
	
	var URI = MakeURI();

	fl.trace("write '" + URI + "'");

	// truncate file
	FLfile.write(URI, "");

	FLfile.write(URI, "<fps>" + gDoc.frameRate + "</fps>\n", "append");		
	
	// Need to go backwards throught the loop because we need the buttom layer first
	for (layerIndex = gNumLayers - 1; layerIndex >= 0; layerIndex--)
	{
	  WriteReanimLayer(layerIndex, URI);
	}

	fl.trace("done writing file");
}

DebugTime("start");

// This disables the annoying "a script in file has been running for a long time" message
fl.showIdleMessage(false); 

var gTimeLine = gDoc.getTimeline();
var gNumFrames = gTimeLine.frameCount;
var gNumLayers = gTimeLine.layerCount;
var gCurrentFrame = gTimeLine.currentFrame;
var gCurrentSelection = gTimeLine.getSelectedFrames();

for (layerIndex = 0; layerIndex < gNumLayers; layerIndex++)
{
	gTimeLine.setSelectedLayers(layerIndex);
	gTimeLine.copyFrames();
	gTimeLine.convertToKeyframes();

	var layername = gTimeLine.layers[layerIndex].name;
	gTimeLine.setSelectedLayers(gTimeLine.layerCount - 1);
	gTimeLine.addNewLayer(layername, "normal", false);
	var newLayerIndex = gTimeLine.layerCount - 1;

	gTimeLine.setSelectedLayers(newLayerIndex);
	gTimeLine.pasteFrames();
	// NOTE: There's a bug in flash where convertToKeyframes fails on pasted frames. ug.
	
	gTimeLine.layers[newLayerIndex].visible = gTimeLine.layers[layerIndex].visible;
	gTimeLine.layers[newLayerIndex].locked = gTimeLine.layers[layerIndex].locked;
	gTimeLine.layers[newLayerIndex].color = gTimeLine.layers[layerIndex].color;
	gTimeLine.layers[newLayerIndex].height = gTimeLine.layers[layerIndex].height;
	gTimeLine.layers[newLayerIndex].layerType = gTimeLine.layers[layerIndex].layerType;
	gTimeLine.layers[newLayerIndex].outline = gTimeLine.layers[layerIndex].outline;
}

try
{
	WriteReanimFile();
}
catch(err)
{
	alert("Export failed.\n\n" + err);
}

// delete temp layers
for (layerIndex = 0; layerIndex < gNumLayers; layerIndex++)
{
  gTimeLine.deleteLayer(0);
}

// Restore selection
gTimeLine.setSelectedFrames(gCurrentSelection);
gTimeLine.currentFrame = gCurrentFrame;

DebugTime("done");
