 | Name                  |    Mean | Baseline | /Mean |    Last | /Mean | 
 | --------------------- | ------: | -------: | ----: | ------: | ----: | 
 | Calibration           |  504 ns |   485 ns | 0.96x |  505 ns | 1.00x | 
 | Enumerate             | 13.5 us |  12.9 us | 0.96x | 11.8 us | 0.87x | 
 | ForLoop               |  159 us |  69.3 us | 0.44x | 71.4 us | 0.45x | 
 | IntegerSum            | 16.9 us |  16.0 us | 0.95x | 14.8 us | 0.88x | 
 | IntegerSumCached      | 6.97 us |  5.61 us | 0.80x | 6.12 us | 0.88x | 
 | DateTimeCached        | 9.14 us |  9.06 us | 0.99x | 8.43 us | 0.92x | 
 | BooleanCached         | 7.70 us |  7.53 us | 0.98x | 8.50 us | 1.10x | 
 | EnumCached            | 8.35 us |  7.01 us | 0.84x | 7.54 us | 0.90x | 
 | StringNulls           | 7.39 us |  8.59 us | 1.16x | 7.93 us | 1.07x | 
 | StringDistinct        | 9.95 us |  10.9 us | 1.10x | 10.1 us | 1.02x | 
 | String                | 84.1 us |  90.0 us | 1.07x | 83.9 us | 1.00x | 
 | String2x              | 94.6 us |  89.0 us | 0.94x | 96.4 us | 1.02x | 
 | ListColumnEnumerate   | 67.0 us |  71.3 us | 1.06x | 71.5 us | 1.07x | 
 | ListColumnFor         |  269 us |   303 us | 1.12x |  280 us | 1.04x | 
 | ListColumnForCached   |  113 us |   116 us | 1.02x |  112 us | 0.99x | 
 | DictionaryReadSuccess |  232 us |   221 us | 0.95x |  237 us | 1.02x | 
 | DictionaryReadFail    |  133 us |   138 us | 1.04x |  131 us | 0.98x | 
 | Dictionary2x          |  162 us |   174 us | 1.07x |  169 us | 1.04x | 
 | DictionaryAddRemove   |  695 us |   721 us | 1.04x |  690 us | 0.99x | 
 | DictionaryInt2x       |  183 us |   202 us | 1.10x |  184 us | 1.00x | 
