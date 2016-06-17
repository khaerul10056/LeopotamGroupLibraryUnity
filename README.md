# LeopotamGroupLibraryUnity
LeopotamGroup library for unity3d engine.

## CODE STRUCTURE

### 'LeopotamGroup' - main library.

***Unity tested version: 5.3.5.***

All code separated to independent subsystems (folders),
you can remove unnecessary code for current project:

----------------------------------------------------------------------------

* Analytics

Google Analytics.

----------------------------------------------------------------------------

* Common

Common helpers, uses by other subsystems.

----------------------------------------------------------------------------

* EditorHelpers

Special helpers: show fps, debug-log output with automatic removing on build, etc.

----------------------------------------------------------------------------

* FX

Visual / audial effect helpers: sound / music manipulations, screen fading.

----------------------------------------------------------------------------

* Gui

Mobile gui. Oriented to be fat free, fast, no or low gc allocation.

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

* Protection

Protection for Int, Long, Float types from in-memory searching.

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