# LeopotamGroupUnity
LeopotamGroup library for unity3d engine

## CODE STRUCTURE

### 'LeopotamGroup' - main library.
All code separated to independent subsystems (folders),
you can remove unnecessary code for current project:

----------------------------------------------------------------------------

* Common
Common helpers, uses by other subsystems. 

----------------------------------------------------------------------------

* EditorHelpers
Special helpers: extract info from editor to use in runtime, show fps, etc. 

----------------------------------------------------------------------------

* FX
Visual / audial effect helpers: sound / music manipulations, screen fading. 

----------------------------------------------------------------------------

* Localization
Localization support with csv support. 

----------------------------------------------------------------------------

* Math
Additional types, 'mersenne twister'-based RNG. 

----------------------------------------------------------------------------

* Mobile
Helpers for mobiles only. 

----------------------------------------------------------------------------

* Notifications
'Waiting' indicator, popup-message indicator. 

----------------------------------------------------------------------------

* Pooling
Pooling support for any prefabs. 

----------------------------------------------------------------------------

* Scripting
Embedded scripting engine, optimized for low gc usage. 

----------------------------------------------------------------------------

* Serialization
Csv deserialization, Json serialization / deserialization with support of
structs and nested objects (lists, arrays, structs, etc). 

----------------------------------------------------------------------------

* Tutorials
Step by step behaviour helpers, useful for creating tutorial or any
other behaviour with ordered / dependent execution. Progress can be saved. 

----------------------------------------------------------------------------

* Tweening
Simple tweening. 

----------------------------------------------------------------------------

### 'LeopotamGroup.Examples' - examples of using main library.


## LICENSE
Copyright (c) 2012-2016 Leopotam <leopotam@gmail.com>

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

----------------------------------------------------------------------------
### The software is double licensed:
* under the terms of the [Attribution-NonCommercial-ShareAlike 4.0](https://creativecommons.org/licenses/by-nc-sa/4.0/)

* under commercial license, email to author for details

----------------------------------------------------------------------------