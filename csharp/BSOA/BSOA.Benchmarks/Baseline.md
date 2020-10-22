 | Name                  |    Mean | Baseline | /Mean |    Last | /Mean | 
 | --------------------- | ------: | -------: | ----: | ------: | ----: | 
 | Calibration           |  485 ns |  2.03 us |  4.2x |       - |     - | 
 | Enumerate             | 12.9 us |  28.0 us |  2.2x | 13.0 us | 1.01x | 
 | ForLoop               | 69.3 us |   287 us |  4.1x | 68.4 us | 0.99x | 
 | IntegerSum            | 16.0 us |  47.0 us |  2.9x | 14.7 us | 0.92x | 
 | IntegerSumCached      | 5.61 us |  19.2 us |  3.4x | 6.41 us | 1.14x | 
 | DateTimeCached        | 9.06 us |  27.8 us |  3.1x | 7.69 us | 0.85x | 
 | BooleanCached         | 7.53 us |  24.6 us |  3.3x | 7.51 us | 1.00x | 
 | EnumCached            | 7.01 us |  21.4 us |  3.1x | 7.09 us | 1.01x | 
 | StringNulls           | 8.59 us |  50.5 us |  5.9x | 8.54 us | 0.99x | 
 | StringDistinct        | 10.9 us |  34.1 us |  3.1x | 9.69 us | 0.89x | 
 | String                | 90.0 us |   202 us |  2.2x | 82.4 us | 0.92x | 
 | String2x              | 89.0 us |   229 us |  2.6x |  100 us | 1.12x | 
 | ListColumnEnumerate   | 71.3 us |   209 us |  2.9x | 68.1 us | 0.96x | 
 | ListColumnFor         |  303 us |  1.03 ms |  3.4x |  285 us | 0.94x | 
 | ListColumnForCached   |  116 us |   502 us |  4.3x |  115 us | 0.99x | 
 | DictionaryReadSuccess |  221 us |   668 us |  3.0x |  225 us | 1.02x | 
 | DictionaryReadFail    |  138 us |   449 us |  3.3x |  158 us | 1.14x | 
 | Dictionary2x          |  174 us |   590 us |  3.4x |  173 us | 1.00x | 
 | DictionaryAddRemove   |  721 us |  1.95 ms |  2.7x |  723 us | 1.00x | 
 | DictionaryInt2x       |  202 us |   573 us |  2.8x |  178 us | 0.88x | 
