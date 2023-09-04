INSERT INTO spotify_playlist (id, name, href) 
VALUES 
    ('3RWXoorVz13dot7I11eiu6', 'Test Playlist', 'https://api.spotify.com/v1/playlists/3RWXoorVz13dot7I11eiu6'),
    ('4jkoiv8phuKuhXboalicqu', 'Test Playlist 2', 'https://api.spotify.com/v1/playlists/4jkoiv8phuKuhXboalicqu'),
    ('1aQvavrk6m4hhEj1NJ0rQB', 'Test Playlist 3', 'https://api.spotify.com/v1/playlists/1aQvavrk6m4hhEj1NJ0rQB'),
    ('4btLcc6A1UmNR7sa2dqlLJ', 'Test Playlist 4', 'https://api.spotify.com/v1/playlists/4btLcc6A1UmNR7sa2dqlLJ');

INSERT INTO spotify_artist (id, name, href) 
VALUES 
    ('4tZwfgrHOc3mvqYlEYSvVi', 'Daft Punk', 'https://api.spotify.com/v1/artists/4tZwfgrHOc3mvqYlEYSvVi'),
    ('0L8ExT028jH3ddEcZwqJJ5', 'Red Hot Chili Peppers', 'https://api.spotify.com/v1/artists/0L8ExT028jH3ddEcZwqJJ5'),
    ('2xaAOVImG2O6lURwqperlD', 'Catfish and the Bottlemen', 'https://api.spotify.com/v1/artists/2xaAOVImG2O6lURwqperlD'),
    ('2DaxqgrOhkeH0fpeiQq2f4', 'Oasis', 'https://api.spotify.com/v1/artists/2DaxqgrOhkeH0fpeiQq2f4');

INSERT INTO spotify_album (id, name, href) 
VALUES 
    ('3Bz2QPL8NLBn1d03jXtNkT', 'Alive 2007', 'https://api.spotify.com/v1/albums/3Bz2QPL8NLBn1d03jXtNkT'),
    ('4m2880jivSbbyEGAKfITCa', 'Random Access Memories', 'https://api.spotify.com/v1/albums/4m2880jivSbbyEGAKfITCa'),
    ('3AMXFnwHWXCvNr5NCCpLZI', 'TRON: Legacy - The Complete Edition (Original Motion Picture Soundtrack)', 'https://api.spotify.com/v1/albums/3AMXFnwHWXCvNr5NCCpLZI'),
    ('2T7DdrOvsqOqU9bGTkjBYu', 'Human After All', 'https://api.spotify.com/v1/albums/2T7DdrOvsqOqU9bGTkjBYu'),
    ('2noRn2Aes5aoNVsU6iWThc', 'Discovery', 'https://api.spotify.com/v1/albums/2noRn2Aes5aoNVsU6iWThc'),
    ('5uRdvUR7xCnHmUW8n64n9y', 'Homework', 'https://api.spotify.com/v1/albums/5uRdvUR7xCnHmUW8n64n9y'),
    ('43otFXrY0bgaq5fB3GrZj6', 'The Getaway', 'https://api.spotify.com/v1/albums/43otFXrY0bgaq5fB3GrZj6'),
    ('7xl50xr9NDkd3i2kBbzsNZ', 'Stadium Arcadium', 'https://api.spotify.com/v1/albums/7xl50xr9NDkd3i2kBbzsNZ'),
    ('6deiaArbeoqp1xPEGdEKp1', 'By the Way (Deluxe Edition)', 'https://api.spotify.com/v1/albums/6deiaArbeoqp1xPEGdEKp1'),
    ('0fLhefnjlIV3pGNF9Wo8CD', 'Californication', 'https://api.spotify.com/v1/albums/0fLhefnjlIV3pGNF9Wo8CD'),
    ('0eELSmJrZpzOKfdO80nJ9r', 'The Balance', 'https://api.spotify.com/v1/albums/0eELSmJrZpzOKfdO80nJ9r'),
    ('07IHAhsG4FnnfHQSb3bbAZ', 'The Ride', 'https://api.spotify.com/v1/albums/07IHAhsG4FnnfHQSb3bbAZ'),
    ('0C0OFASoQC57yC12vQhCwN', 'The Balcony', 'https://api.spotify.com/v1/albums/0C0OFASoQC57yC12vQhCwN'),
    ('3AMHMM2aNG6k3d7ybcQ5bY', 'Definitely Maybe (Deluxe Edition Remastered)', 'https://api.spotify.com/v1/albums/3AMHMM2aNG6k3d7ybcQ5bY'),
    ('6tOe4eAF8xNhEkl9WyvsE4', '(What''s The Story) Morning Glory? (Deluxe Remastered Edition)', 'https://api.spotify.com/v1/albums/6tOe4eAF8xNhEkl9WyvsE4'),
    ('4XBCWqCXqCdN72K9SklIjy', 'Be Here Now (Deluxe Remastered Edition)', 'https://api.spotify.com/v1/albums/4XBCWqCXqCdN72K9SklIjy'),
    ('2EVWJRhbXWsSm7a6jdKv8O', 'Heathen Chemistry', 'https://api.spotify.com/v1/albums/2EVWJRhbXWsSm7a6jdKv8O');

INSERT INTO spotify_track (id, spotify_artist_id, spotify_album_id, name, length) 
VALUES 
    -- Daft Punk Tracks
    ('0q9zz6nP5izcUnfYndfVX6', '4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT', 'Robot Rock / Oh Yeah', 387506),
    ('0yf4PlZ8rG3MrnqsmTzvMp', '4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT', 'Touch It / Technologic', 329653),
    ('5pid7wpFMJvXvlYETOIUzo', '4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT', 'Television Rules the Nation / Crescendolls', 290840),
    ('1DtOKrgREeQ9va6f3FAfXf', '4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT', 'Too Long / Steam Machine', 421600),
    ('0iYGI01MEFu9ohSNuMVpwL', '4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT', 'Around the World / Harder, Better, Faster, Stronger', 342626),
    ('5CYM628A5vNaPlx2hy6Drs', '4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT', 'Burnin'' / Too Long', 431506),

    ('0dEIca2nhcxDUV8C5QkPYb', '4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa', 'Give Life Back to Music', 275386),
    ('3ctALmweZBapfBdFiIVpji', '4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa', 'The Game of Love', 322146),
    ('0oks4FnzhNp5QPTZtoet7c', '4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa', 'Giorgio by Moroder', 544626),
    ('7Bxv0WL7UC6WwQpk9TzdMJ', '4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa', 'Within', 228506),
    ('2cGxRwrMyEAp8dEbuZaVv6', '4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa', 'Instant Crush (feat. Julian Casablancas)', 337560),
    ('5CMjjywI0eZMixPeqNd75R', '4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa', 'Lose Yourself to Dance (feat. Pharrell Williams)', 353893),

    ('2tRWMCijEFsGPDxBgHxHre', '4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI', 'The Grid', 96739),
    ('3zL0LAsSh3dTO73dSOKWkr', '4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI', 'The Son of Flynn', 95226),
    ('6dooKqgWKBVwQLLarxJPDM', '4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI', 'Recognizer', 157719),
    ('6NbukMzsdx888nymIiWKlV', '4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI', 'Armory', 122975),
    ('4csD9dmdLHnarNyu1wG8Iv', '4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI', 'Arena', 93236),
    ('70RkgofUfQHLl2FT2Mx5zq', '4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI', 'Rinzler', 137635),

    ('3aCKAkMx3yfaj3AO5Gz47e', '4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu', 'Human After All', 319879),
    ('0UZRFYMoz9xmeE2AQUhTDl', '4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu', 'The Prime Time of Your Life', 263240),
    ('7LL40F6YdZgeiQ6en1c7Lk', '4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu', 'Robot Rock', 287720),
    ('60HSQkYSlJVtdRdHgzRsXz', '4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu', 'Steam Machine', 321186),
    ('4ABWPP59ItFKykdaDF09K5', '4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu', 'Make Love', 289680),
    ('73MAeHX5sqLYfuYclsrvHc', '4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu', 'The Brainwasher', 248400),

    ('0DiWol3AO6WpXZgp0goxAV', '4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc', 'One More Time', 320357),
    ('3H3cOQ6LBLSvmcaV7QkZEu', '4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc', 'Aerodynamic', 212546),
    ('2VEZx7NWsZ1D0eJ4uv5Fym', '4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc', 'Digital Love', 301373),
    ('5W3cjX2J3tjhG8zb6u0qHn', '4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc', 'Harder, Better, Faster, Stronger', 224693),
    ('6vuPZX9fWESg5js2JFTQRJ', '4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc', 'Crescendolls', 211640),
    ('63JXZZRbmzooashakb0zbu', '4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc', 'Nightvision', 104466),

    ('7fYIXtdOUl8WqXAqQGyIHQ', '4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y', 'Daftendirekt', 164560),
    ('7dTWkvPOPgbGuMk4HDxNpY', '4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y', 'WDPK 83.7 FM', 28333),
    ('5pgZpHqfv4TSomtkfGZGrG', '4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y', 'Revolution 909', 335026),
    ('0MyY4WcN7DIfbSmp5yej5z', '4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y', 'Da Funk', 328680),
    ('6nQy5XEEEJKu8FE1FS2Wbt', '4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y', 'Phoenix', 297106),
    ('78H72MElkOY9cRnaudxZFY', '4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y', 'Fresh', 244400),

    -- Red Hot Chili Peppers Tracks
    ('3bIQIx7hveYPQDdhjZ1kcq', '0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6', 'The Getaway', 250386),
    ('2oaK4JLVnmRGIO9ytBE1bt', '0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6', 'Dark Necessities', 302000),
    ('0cv2LgkvEoQiGgFWcZaAMA', '0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6', 'We Turn Red', 200466),
    ('0pjCkLjbgSLn5c0Ilwuv8z', '0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6', 'The Longest Wave', 211520),
    ('2XTkpF9T2PKvcLgamGJGx1', '0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6', 'Goodbye Angels', 268733),
    ('6GsP3uMCd0Dn5T37C93waZ', '0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6', 'Sick Love', 221440),

    ('10Nmj3JCNoMeBQ87uw5j8k', '0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ', 'Dani California', 282160),
    ('2aibwv5hGXSgw7Yru8IYTO', '0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ', 'Snow (Hey Oh)', 334666),
    ('3SoDB59Y7dSZLSDBiNJ6o2', '0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ', 'Charlie', 277533),
    ('4y84ILALZSa4LyP6H7NVjR', '0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ', 'Stadium Arcadium', 314773),
    ('5f2ZVFERwwh3asebmurZEf', '0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ', 'Hump de Bump', 213093),
    ('3gvyksxkLbyKwi0WjCiPXE', '0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ', 'She''s Only 18', 205266),

    ('1f2V8U1BiWaC9aJWmpOARe', '0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1', 'By the Way', 216933),
    ('39badcyKTjOtBvv4aywpfs', '0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1', 'Universally Speaking', 256959),
    ('42z5BOO8cJda4qWBnHFLQV', '0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1', 'This Is the Place', 257693),
    ('1iFIZUVDBCCkWe705FLXto', '0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1', 'Dosed', 311866),
    ('308kjPoIVBzDNExO54sqAi', '0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1', 'Don''t Forget Me', 277160),
    ('1ndGB6rvxKYN9seCYO1dTF', '0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1', 'The Zephyr Song', 231933),

    ('1Y6DGcTCuMAtw8KB3h4W3q', '0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD', 'Around the World', 239213),
    ('3ZwxczCIlt4nA7czQaugvM', '0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD', 'Parallel Universe', 269373),
    ('2QOMGq8wVTZbLmh7McrvgF', '0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD', 'Scar Tissue', 215906),
    ('3CeYdUfGPCjKMDYyI1PpCh', '0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD', 'Otherside', 255373),
    ('3s3oiCHAHLWmKZUYk1ozJG', '0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD', 'Get on Top', 198066),
    ('34KTEhpPjq6IAgQg2yzJAL', '0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD', 'Californication', 329733),

    -- Catfish and the Bottlemen Tracks
    ('2VcS3oKcOPkubN9LVzZ96l', '2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r', 'Longshot', 232960),
    ('27xKntLxqf0HDGVdcNIkcY', '2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r', 'Fluctuate', 192053),
    ('0ZfEeIu7CNHQhbCTiVv3cx', '2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r', '2all', 188440),
    ('21slhimb1blAmvpjq0l8rh', '2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r', 'Conversation', 211986),
    ('2c8U7Mlax6uIhI9aM4YdpH', '2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r', 'Sidetrack', 200520),
    ('4LdImt7hg85uPpUOPKlLAy', '2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r', 'Encore', 165506),

    ('5ykbOijJEfRhuo2Td1m0Qd', '2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ', '7', 256306),
    ('2sewj0rFvlr3aEM3bGy12n', '2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ', 'Twice', 196946),
    ('60W7Co2AoP5VVG5Gwu6p5P', '2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ', 'Soundcheck', 261560),
    ('1IMtCtSqlw3VJv3IvCxkaz', '2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ', 'Postpone', 245533),
    ('1JblvWxcwHXMLth0c6ssFy', '2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ', 'Anything', 249480),
    ('28VT0090inPlN6bfxoVdmB', '2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ', 'Glasgow', 157000),

    ('7lnXzMcgaK7CnzaQ7wj6k0', '2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN', 'Homesick', 148092),
    ('1MHYAqWWdDRePmnqORynrq', '2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN', 'Kathleen', 160886),
    ('1B241LRKmK6qDDTZfUajmm', '2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN', 'Cocoon', 236887),
    ('2PAeLLcnw42x5ZszOfFz50', '2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN', 'Fallout', 210405),
    ('1rcu88dzWE5GyqtpuWvd0C', '2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN', 'Pacifier', 237533),
    ('4hEhOvEz9tulJQXZ7hiqkz', '2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN', 'Hourglass', 138154),

    -- Oasis Tracks
    ('4bQHPFjRT6O1KdMCd4cD9u', '2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY', 'Rock ''n'' Roll Star - Remastered', 322946),
    ('7HHNKKfD9oNshMTyklBeWu', '2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY', 'Shakermaker - Remastered', 308280),
    ('6TlQ5fbojNRuG0hPQMbxeW', '2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY', 'Live Forever - Remastered', 276666),
    ('10fOKTNaGzajbX5SdS00Ut', '2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY', 'Up In The Sky - Remastered', 268000),
    ('20EkniSUbhZ86v1Oc0hhUI', '2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY', 'Columbia - Remastered', 377400),
    ('4jJfa4mO5JjV9Tz2aAxE2M', '2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY', 'Supersonic - Remastered', 283786),

    ('1KcFLmFGCfkkAsooF6FJBr', '2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4', 'Hello - Remastered', 203186),
    ('3czgcbI4YZ4Gg29DOYFAnd', '2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4', 'Roll With It - Remastered', 240093),
    ('7ygpwy2qP3NbrxVkHvUhXY', '2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4', 'Wonderwall - Remastered', 258773),
    ('12dU3vAh6AFoJkisorfoUl', '2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4', 'Don''t Look Back In Anger - Remastered', 289560),
    ('4FDEDR99kqoGZOV88Wpnpg', '2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4', 'Hey Now! - Remastered', 341906),
    ('1xxPQaR29dAf3yxaafByeD', '2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4', 'Some Might Say - Remastered', 327920),

    ('4yWTh6qETpN1Vlfg5LBHEV', '2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy', 'D''You Know What I Mean? - Remastered', 463066),
    ('2qQBPCjY7WQiqO4LEp9wsU', '2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy', 'My Big Mouth - Remastered', 311333),
    ('4B0uIsmygOfQpNSQzanJ5i', '2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy', 'Magic Pie - Remastered', 430373),
    ('5WTPhYmwwojFP73O7p3Izi', '2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy', 'Stand By Me - Remastered', 355866),
    ('5g4IouEbo38FZi8M1Ga4ey', '2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy', 'I Hope, I Think, I Know - Remastered', 262400),
    ('1MkwGdfDRDpDe0y2LkHAFO', '2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy', 'The Girl In The Dirty Shirt - Remastered', 349640),

    ('7IuCCezGsyTH9LU4IVL57s', '2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O', 'The Hindu Times', 226400),
    ('3ujwIruj897uySuJuwoPQN', '2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O', 'Force Of Nature', 291693),
    ('4RhOMvp9ZPyTuXwLCWvi8O', '2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O', 'Hung In A Bad Place', 208666),
    ('0SlpFHdk4UHBDzCEoXzy14', '2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O', 'Stop Crying Your Heart Out', 303120),
    ('2tBxJ43XiwEzhf7Xalc5UG', '2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O', 'Songbird', 127600),
    ('45AQ5zbSJ0j1nM9daSWeq8', '2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O', 'Little By Little', 292853);

INSERT INTO skipped_track (skipped_date, spotify_playlist_id, spotify_track_id) 
VALUES 
    -- Test Playlist skips
    (1692958242, '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (1692958289, '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (1692958307, '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (1692958325, '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (1692957617, '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (1692957620, '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),

    (1692958265, '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (1692958293, '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (1692958312, '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (1692958329, '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (1692957665, '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),

    (1692958272, '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),
    (1692958297, '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),
    (1692958315, '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),
    (1692958334, '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),

    (1692958278, '3RWXoorVz13dot7I11eiu6', '73MAeHX5sqLYfuYclsrvHc'),
    (1692958300, '3RWXoorVz13dot7I11eiu6', '73MAeHX5sqLYfuYclsrvHc'),
    (1692958319, '3RWXoorVz13dot7I11eiu6', '73MAeHX5sqLYfuYclsrvHc'),

    (1692958281, '3RWXoorVz13dot7I11eiu6', '63JXZZRbmzooashakb0zbu'),
    (1692958304, '3RWXoorVz13dot7I11eiu6', '63JXZZRbmzooashakb0zbu'),

    (1692958286, '3RWXoorVz13dot7I11eiu6', '78H72MElkOY9cRnaudxZFY'),

    -- Test Playlist 2 skips
    (1692958344, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (1692958366, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (1692958384, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (1692958400, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (1692958410, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (1692958420, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (1692958427, '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),

    (1692958354, '4jkoiv8phuKuhXboalicqu', '5f2ZVFERwwh3asebmurZEf'),
    (1692958369, '4jkoiv8phuKuhXboalicqu', '5f2ZVFERwwh3asebmurZEf'),
    (1692958388, '4jkoiv8phuKuhXboalicqu', '5f2ZVFERwwh3asebmurZEf'),

    (1692958358, '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (1692958373, '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (1692958392, '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (1692958403, '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (1692958413, '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),

    (1692958362, '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (1692958377, '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (1692958396, '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (1692958407, '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (1692958416, '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (1692959186, '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),

    -- Test Playlist 3 skips
    (1692958614, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958641, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958665, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958696, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958713, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958725, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958732, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (1692958738, '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),

    (1692958618, '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (1692958645, '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (1692958674, '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (1692958699, '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (1692958716, '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),

    (1692958623, '1aQvavrk6m4hhEj1NJ0rQB', '4hEhOvEz9tulJQXZ7hiqkz'),
    (1692958648, '1aQvavrk6m4hhEj1NJ0rQB', '4hEhOvEz9tulJQXZ7hiqkz'),
    (1692958683, '1aQvavrk6m4hhEj1NJ0rQB', '4hEhOvEz9tulJQXZ7hiqkz'),

    (1692958626, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958653, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958687, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958702, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958721, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958729, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958735, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958424, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (1692958741, '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),

    (1692958630, '1aQvavrk6m4hhEj1NJ0rQB', '2PAeLLcnw42x5ZszOfFz50'),
    (1692958658, '1aQvavrk6m4hhEj1NJ0rQB', '2PAeLLcnw42x5ZszOfFz50'),

    (1692958634, '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),
    (1692958662, '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),
    (1692958691, '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),
    (1692958709, '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),

    -- Test Playlist 3 skips
    (1692958913, '4btLcc6A1UmNR7sa2dqlLJ', '7HHNKKfD9oNshMTyklBeWu'),
    (1692958943, '4btLcc6A1UmNR7sa2dqlLJ', '7HHNKKfD9oNshMTyklBeWu'),
    (1692959039, '4btLcc6A1UmNR7sa2dqlLJ', '7HHNKKfD9oNshMTyklBeWu'),

    (1692958920, '4btLcc6A1UmNR7sa2dqlLJ', '20EkniSUbhZ86v1Oc0hhUI'),
    (1692958947, '4btLcc6A1UmNR7sa2dqlLJ', '20EkniSUbhZ86v1Oc0hhUI'),

    (1692958923, '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (1692958951, '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (1692959043, '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (1692959057, '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (1692959076, '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (1692959083, '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),

    (1692958926, '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),
    (1692958954, '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),
    (1692959047, '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),
    (1692959062, '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),

    (1692958930, '4btLcc6A1UmNR7sa2dqlLJ', '5g4IouEbo38FZi8M1Ga4ey'),

    (1692958934, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (1692958959, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (1692959051, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (1692959067, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (1692959080, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (1692959087, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (1692959091, '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),

    (1692958938, '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG'),
    (1692958962, '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG'),
    (1692959054, '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG'),
    (1692959073, '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG');