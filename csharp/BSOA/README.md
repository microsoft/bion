
# BSOA
BSOA is a framework for creating object models which use the "struct-of-arrays" (SoA) design pattern.

BSOA object models take up substantially less memory than normal object models and support binary serialization with gigabyte-per-second read and write speeds.

In our original use case, SARIF, the object model typically takes 75% less memory, the binary file is 33% smaller than unindented JSON, and logs in the binary format can be read and written 100x faster than the JSON form.

BSOA is currently a pre-release, and is used internally within Microsoft to load very large datasets quickly into familiar object model representations.

See [BSOA Introduction](https://github.com/microsoft/bion/wiki/BSOA-Introduction) for more details.
