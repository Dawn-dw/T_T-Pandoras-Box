#version 330 core

in vec2 TexCoord;

out vec4 FragColor;

uniform int uType = 0;
uniform float uSpeed = 1;
uniform float uTime = 0;
uniform float uSize = 1;
uniform float uWidth = 5;
uniform vec4 fillColor = vec4(1.0, 1.0, 1.0, 1.0);
uniform vec4 emptyColor = vec4(0.0, 0.0, 0.0, 0.0);

vec3 hsv2rgb(vec3 c) 
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

vec3 hueShift(vec3 color, float hue) 
{
    const vec3 k = vec3(0.57735, 0.57735, 0.57735);
    float cosAngle = cos(hue);
    return vec3(color * cosAngle + cross(k, color) * sin(hue) + k * dot(k, color) * (1.0 - cosAngle));
}

vec4 getFragmentColor()
{
    vec2 pixelCoord = TexCoord * uSize;
    vec2 centerPixel = vec2(0.5) * uSize;

    float distFromCenter = distance(pixelCoord, centerPixel);

    float transitionWidth = uWidth + 2;
    float end = uSize * 0.5;
    float mid = end - transitionWidth;
    float start = mid - transitionWidth;

    if(distFromCenter < start)
    {
        return emptyColor;
    }
    else if(distFromCenter >= start && distFromCenter < mid)
    {
        float t = (distFromCenter - start) / transitionWidth;
        return mix(emptyColor, fillColor, t);
    }
    else if(distFromCenter >= mid && distFromCenter < end)
    {
        float t = (end - distFromCenter) / transitionWidth;
        return mix(emptyColor, fillColor, t);
    }
    else
    {
        return emptyColor;
    }
}

vec3 getAniamtedHueColor()
{
    vec2 centeredCoord = TexCoord - vec2(0.5, 0.5);
    float angle = atan(centeredCoord.y, centeredCoord.x) / (2.0 * 3.14159265);
    float hue = mod(angle + uTime * uSpeed, 1.0);
    vec3 hsvColor = vec3(hue, 1.0, 1.0);
    return hsv2rgb(hsvColor);
}

vec3 getHueColor()
{
    float hue = sin(uTime * uSpeed);
    return hueShift(fillColor.rgb, hue);
}

vec4 getColor(vec4 color)
{
    if(uType == 1)
    {
        return vec4(getHueColor(), color.a);
    }
    else if(uType == 2)
    {
        return vec4(getAniamtedHueColor(), color.a);
    }
    else
    {
        return color;
    }
}

void main()
{
    vec4 fragmentColor = getFragmentColor();
    
    if(fragmentColor.a <= 0)
    {
        FragColor = fragmentColor;
        return;
    }
    
    FragColor = getColor(fragmentColor);
    if(FragColor.a > 0.8){
        FragColor.a = 1;
    }
    else{
        FragColor.a = FragColor.a / 0.8;
    }
}
