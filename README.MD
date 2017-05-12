Use it to clean the beginning of a JPEG.



It will look at the bytes until it finds the signature of a JPEG (0xFF 0xD8). Remove everything before it and put it in a JPEG.



Command line: `cleanjpeg.exe <originFilePath> <destinationFilePath>`