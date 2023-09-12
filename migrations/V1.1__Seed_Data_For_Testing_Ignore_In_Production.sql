INSERT INTO playlists (id) 
VALUES 
    ('3RWXoorVz13dot7I11eiu6'),
    ('4jkoiv8phuKuhXboalicqu'),
    ('1aQvavrk6m4hhEj1NJ0rQB'),
    ('4btLcc6A1UmNR7sa2dqlLJ'),
    ('0creVHKMskko3Kv9T2pPQA');

INSERT INTO artists (id, name, href) 
VALUES 
    ('4tZwfgrHOc3mvqYlEYSvVi', 'Daft Punk', 'https://open.spotify.com/artist/4tZwfgrHOc3mvqYlEYSvVi'),
    ('0L8ExT028jH3ddEcZwqJJ5', 'Red Hot Chili Peppers', 'https://open.spotify.com/artist/0L8ExT028jH3ddEcZwqJJ5'),
    ('2xaAOVImG2O6lURwqperlD', 'Catfish and the Bottlemen', 'https://open.spotify.com/artist/2xaAOVImG2O6lURwqperlD'),
    ('2DaxqgrOhkeH0fpeiQq2f4', 'Oasis', 'https://open.spotify.com/artist/2DaxqgrOhkeH0fpeiQq2f4');

INSERT INTO albums (id, name, href) 
VALUES 
    ('3Bz2QPL8NLBn1d03jXtNkT', 'Alive 2007', 'https://open.spotify.com/album/3Bz2QPL8NLBn1d03jXtNkT'),
    ('4m2880jivSbbyEGAKfITCa', 'Random Access Memories', 'https://open.spotify.com/album/4m2880jivSbbyEGAKfITCa'),
    ('3AMXFnwHWXCvNr5NCCpLZI', 'TRON: Legacy - The Complete Edition (Original Motion Picture Soundtrack)', 'https://open.spotify.com/album/3AMXFnwHWXCvNr5NCCpLZI'),
    ('2T7DdrOvsqOqU9bGTkjBYu', 'Human After All', 'https://open.spotify.com/album/2T7DdrOvsqOqU9bGTkjBYu'),
    ('2noRn2Aes5aoNVsU6iWThc', 'Discovery', 'https://open.spotify.com/album/2noRn2Aes5aoNVsU6iWThc'),
    ('5uRdvUR7xCnHmUW8n64n9y', 'Homework', 'https://open.spotify.com/album/5uRdvUR7xCnHmUW8n64n9y'),
    ('43otFXrY0bgaq5fB3GrZj6', 'The Getaway', 'https://open.spotify.com/album/43otFXrY0bgaq5fB3GrZj6'),
    ('7xl50xr9NDkd3i2kBbzsNZ', 'Stadium Arcadium', 'https://open.spotify.com/album/7xl50xr9NDkd3i2kBbzsNZ'),
    ('6deiaArbeoqp1xPEGdEKp1', 'By the Way (Deluxe Edition)', 'https://open.spotify.com/album/6deiaArbeoqp1xPEGdEKp1'),
    ('0fLhefnjlIV3pGNF9Wo8CD', 'Californication', 'https://open.spotify.com/album/0fLhefnjlIV3pGNF9Wo8CD'),
    ('0eELSmJrZpzOKfdO80nJ9r', 'The Balance', 'https://open.spotify.com/album/0eELSmJrZpzOKfdO80nJ9r'),
    ('07IHAhsG4FnnfHQSb3bbAZ', 'The Ride', 'https://open.spotify.com/album/07IHAhsG4FnnfHQSb3bbAZ'),
    ('0C0OFASoQC57yC12vQhCwN', 'The Balcony', 'https://open.spotify.com/album/0C0OFASoQC57yC12vQhCwN'),
    ('3AMHMM2aNG6k3d7ybcQ5bY', 'Definitely Maybe (Deluxe Edition Remastered)', 'https://open.spotify.com/album/3AMHMM2aNG6k3d7ybcQ5bY'),
    ('6tOe4eAF8xNhEkl9WyvsE4', '(What''s The Story) Morning Glory? (Deluxe Remastered Edition)', 'https://open.spotify.com/album/6tOe4eAF8xNhEkl9WyvsE4'),
    ('4XBCWqCXqCdN72K9SklIjy', 'Be Here Now (Deluxe Remastered Edition)', 'https://open.spotify.com/album/4XBCWqCXqCdN72K9SklIjy'),
    ('2EVWJRhbXWsSm7a6jdKv8O', 'Heathen Chemistry', 'https://open.spotify.com/album/2EVWJRhbXWsSm7a6jdKv8O');

INSERT INTO tracks (id, album_id, name, length) 
VALUES 
    -- Daft Punk Tracks
    ('0q9zz6nP5izcUnfYndfVX6', '3Bz2QPL8NLBn1d03jXtNkT', 'Robot Rock / Oh Yeah', 387506),
    ('0yf4PlZ8rG3MrnqsmTzvMp', '3Bz2QPL8NLBn1d03jXtNkT', 'Touch It / Technologic', 329653),
    ('5pid7wpFMJvXvlYETOIUzo', '3Bz2QPL8NLBn1d03jXtNkT', 'Television Rules the Nation / Crescendolls', 290840),
    ('1DtOKrgREeQ9va6f3FAfXf', '3Bz2QPL8NLBn1d03jXtNkT', 'Too Long / Steam Machine', 421600),
    ('0iYGI01MEFu9ohSNuMVpwL', '3Bz2QPL8NLBn1d03jXtNkT', 'Around the World / Harder, Better, Faster, Stronger', 342626),
    ('5CYM628A5vNaPlx2hy6Drs', '3Bz2QPL8NLBn1d03jXtNkT', 'Burnin'' / Too Long', 431506),

    ('0dEIca2nhcxDUV8C5QkPYb', '4m2880jivSbbyEGAKfITCa', 'Give Life Back to Music', 275386),
    ('3ctALmweZBapfBdFiIVpji', '4m2880jivSbbyEGAKfITCa', 'The Game of Love', 322146),
    ('0oks4FnzhNp5QPTZtoet7c', '4m2880jivSbbyEGAKfITCa', 'Giorgio by Moroder', 544626),
    ('7Bxv0WL7UC6WwQpk9TzdMJ', '4m2880jivSbbyEGAKfITCa', 'Within', 228506),
    ('2cGxRwrMyEAp8dEbuZaVv6', '4m2880jivSbbyEGAKfITCa', 'Instant Crush (feat. Julian Casablancas)', 337560),
    ('5CMjjywI0eZMixPeqNd75R', '4m2880jivSbbyEGAKfITCa', 'Lose Yourself to Dance (feat. Pharrell Williams)', 353893),

    ('2tRWMCijEFsGPDxBgHxHre', '3AMXFnwHWXCvNr5NCCpLZI', 'The Grid', 96739),
    ('3zL0LAsSh3dTO73dSOKWkr', '3AMXFnwHWXCvNr5NCCpLZI', 'The Son of Flynn', 95226),
    ('6dooKqgWKBVwQLLarxJPDM', '3AMXFnwHWXCvNr5NCCpLZI', 'Recognizer', 157719),
    ('6NbukMzsdx888nymIiWKlV', '3AMXFnwHWXCvNr5NCCpLZI', 'Armory', 122975),
    ('4csD9dmdLHnarNyu1wG8Iv', '3AMXFnwHWXCvNr5NCCpLZI', 'Arena', 93236),
    ('70RkgofUfQHLl2FT2Mx5zq', '3AMXFnwHWXCvNr5NCCpLZI', 'Rinzler', 137635),

    ('3aCKAkMx3yfaj3AO5Gz47e', '2T7DdrOvsqOqU9bGTkjBYu', 'Human After All', 319879),
    ('0UZRFYMoz9xmeE2AQUhTDl', '2T7DdrOvsqOqU9bGTkjBYu', 'The Prime Time of Your Life', 263240),
    ('7LL40F6YdZgeiQ6en1c7Lk', '2T7DdrOvsqOqU9bGTkjBYu', 'Robot Rock', 287720),
    ('60HSQkYSlJVtdRdHgzRsXz', '2T7DdrOvsqOqU9bGTkjBYu', 'Steam Machine', 321186),
    ('4ABWPP59ItFKykdaDF09K5', '2T7DdrOvsqOqU9bGTkjBYu', 'Make Love', 289680),
    ('73MAeHX5sqLYfuYclsrvHc', '2T7DdrOvsqOqU9bGTkjBYu', 'The Brainwasher', 248400),

    ('0DiWol3AO6WpXZgp0goxAV', '2noRn2Aes5aoNVsU6iWThc', 'One More Time', 320357),
    ('3H3cOQ6LBLSvmcaV7QkZEu', '2noRn2Aes5aoNVsU6iWThc', 'Aerodynamic', 212546),
    ('2VEZx7NWsZ1D0eJ4uv5Fym', '2noRn2Aes5aoNVsU6iWThc', 'Digital Love', 301373),
    ('5W3cjX2J3tjhG8zb6u0qHn', '2noRn2Aes5aoNVsU6iWThc', 'Harder, Better, Faster, Stronger', 224693),
    ('6vuPZX9fWESg5js2JFTQRJ', '2noRn2Aes5aoNVsU6iWThc', 'Crescendolls', 211640),
    ('63JXZZRbmzooashakb0zbu', '2noRn2Aes5aoNVsU6iWThc', 'Nightvision', 104466),

    ('7fYIXtdOUl8WqXAqQGyIHQ', '5uRdvUR7xCnHmUW8n64n9y', 'Daftendirekt', 164560),
    ('7dTWkvPOPgbGuMk4HDxNpY', '5uRdvUR7xCnHmUW8n64n9y', 'WDPK 83.7 FM', 28333),
    ('5pgZpHqfv4TSomtkfGZGrG', '5uRdvUR7xCnHmUW8n64n9y', 'Revolution 909', 335026),
    ('0MyY4WcN7DIfbSmp5yej5z', '5uRdvUR7xCnHmUW8n64n9y', 'Da Funk', 328680),
    ('6nQy5XEEEJKu8FE1FS2Wbt', '5uRdvUR7xCnHmUW8n64n9y', 'Phoenix', 297106),
    ('78H72MElkOY9cRnaudxZFY', '5uRdvUR7xCnHmUW8n64n9y', 'Fresh', 244400),

    -- Red Hot Chili Peppers Tracks
    ('3bIQIx7hveYPQDdhjZ1kcq', '43otFXrY0bgaq5fB3GrZj6', 'The Getaway', 250386),
    ('2oaK4JLVnmRGIO9ytBE1bt', '43otFXrY0bgaq5fB3GrZj6', 'Dark Necessities', 302000),
    ('0cv2LgkvEoQiGgFWcZaAMA', '43otFXrY0bgaq5fB3GrZj6', 'We Turn Red', 200466),
    ('0pjCkLjbgSLn5c0Ilwuv8z', '43otFXrY0bgaq5fB3GrZj6', 'The Longest Wave', 211520),
    ('2XTkpF9T2PKvcLgamGJGx1', '43otFXrY0bgaq5fB3GrZj6', 'Goodbye Angels', 268733),
    ('6GsP3uMCd0Dn5T37C93waZ', '43otFXrY0bgaq5fB3GrZj6', 'Sick Love', 221440),

    ('10Nmj3JCNoMeBQ87uw5j8k', '7xl50xr9NDkd3i2kBbzsNZ', 'Dani California', 282160),
    ('2aibwv5hGXSgw7Yru8IYTO', '7xl50xr9NDkd3i2kBbzsNZ', 'Snow (Hey Oh)', 334666),
    ('3SoDB59Y7dSZLSDBiNJ6o2', '7xl50xr9NDkd3i2kBbzsNZ', 'Charlie', 277533),
    ('4y84ILALZSa4LyP6H7NVjR', '7xl50xr9NDkd3i2kBbzsNZ', 'Stadium Arcadium', 314773),
    ('5f2ZVFERwwh3asebmurZEf', '7xl50xr9NDkd3i2kBbzsNZ', 'Hump de Bump', 213093),
    ('3gvyksxkLbyKwi0WjCiPXE', '7xl50xr9NDkd3i2kBbzsNZ', 'She''s Only 18', 205266),

    ('1f2V8U1BiWaC9aJWmpOARe', '6deiaArbeoqp1xPEGdEKp1', 'By the Way', 216933),
    ('39badcyKTjOtBvv4aywpfs', '6deiaArbeoqp1xPEGdEKp1', 'Universally Speaking', 256959),
    ('42z5BOO8cJda4qWBnHFLQV', '6deiaArbeoqp1xPEGdEKp1', 'This Is the Place', 257693),
    ('1iFIZUVDBCCkWe705FLXto', '6deiaArbeoqp1xPEGdEKp1', 'Dosed', 311866),
    ('308kjPoIVBzDNExO54sqAi', '6deiaArbeoqp1xPEGdEKp1', 'Don''t Forget Me', 277160),
    ('1ndGB6rvxKYN9seCYO1dTF', '6deiaArbeoqp1xPEGdEKp1', 'The Zephyr Song', 231933),

    ('1Y6DGcTCuMAtw8KB3h4W3q', '0fLhefnjlIV3pGNF9Wo8CD', 'Around the World', 239213),
    ('3ZwxczCIlt4nA7czQaugvM', '0fLhefnjlIV3pGNF9Wo8CD', 'Parallel Universe', 269373),
    ('2QOMGq8wVTZbLmh7McrvgF', '0fLhefnjlIV3pGNF9Wo8CD', 'Scar Tissue', 215906),
    ('3CeYdUfGPCjKMDYyI1PpCh', '0fLhefnjlIV3pGNF9Wo8CD', 'Otherside', 255373),
    ('3s3oiCHAHLWmKZUYk1ozJG', '0fLhefnjlIV3pGNF9Wo8CD', 'Get on Top', 198066),
    ('34KTEhpPjq6IAgQg2yzJAL', '0fLhefnjlIV3pGNF9Wo8CD', 'Californication', 329733),

    -- Catfish and the Bottlemen Tracks
    ('2VcS3oKcOPkubN9LVzZ96l', '0eELSmJrZpzOKfdO80nJ9r', 'Longshot', 232960),
    ('27xKntLxqf0HDGVdcNIkcY', '0eELSmJrZpzOKfdO80nJ9r', 'Fluctuate', 192053),
    ('0ZfEeIu7CNHQhbCTiVv3cx', '0eELSmJrZpzOKfdO80nJ9r', '2all', 188440),
    ('21slhimb1blAmvpjq0l8rh', '0eELSmJrZpzOKfdO80nJ9r', 'Conversation', 211986),
    ('2c8U7Mlax6uIhI9aM4YdpH', '0eELSmJrZpzOKfdO80nJ9r', 'Sidetrack', 200520),
    ('4LdImt7hg85uPpUOPKlLAy', '0eELSmJrZpzOKfdO80nJ9r', 'Encore', 165506),

    ('5ykbOijJEfRhuo2Td1m0Qd', '07IHAhsG4FnnfHQSb3bbAZ', '7', 256306),
    ('2sewj0rFvlr3aEM3bGy12n', '07IHAhsG4FnnfHQSb3bbAZ', 'Twice', 196946),
    ('60W7Co2AoP5VVG5Gwu6p5P', '07IHAhsG4FnnfHQSb3bbAZ', 'Soundcheck', 261560),
    ('1IMtCtSqlw3VJv3IvCxkaz', '07IHAhsG4FnnfHQSb3bbAZ', 'Postpone', 245533),
    ('1JblvWxcwHXMLth0c6ssFy', '07IHAhsG4FnnfHQSb3bbAZ', 'Anything', 249480),
    ('28VT0090inPlN6bfxoVdmB', '07IHAhsG4FnnfHQSb3bbAZ', 'Glasgow', 157000),

    ('7lnXzMcgaK7CnzaQ7wj6k0', '0C0OFASoQC57yC12vQhCwN', 'Homesick', 148092),
    ('1MHYAqWWdDRePmnqORynrq', '0C0OFASoQC57yC12vQhCwN', 'Kathleen', 160886),
    ('1B241LRKmK6qDDTZfUajmm', '0C0OFASoQC57yC12vQhCwN', 'Cocoon', 236887),
    ('2PAeLLcnw42x5ZszOfFz50', '0C0OFASoQC57yC12vQhCwN', 'Fallout', 210405),
    ('1rcu88dzWE5GyqtpuWvd0C', '0C0OFASoQC57yC12vQhCwN', 'Pacifier', 237533),
    ('4hEhOvEz9tulJQXZ7hiqkz', '0C0OFASoQC57yC12vQhCwN', 'Hourglass', 138154),

    -- Oasis Tracks
    ('4bQHPFjRT6O1KdMCd4cD9u', '3AMHMM2aNG6k3d7ybcQ5bY', 'Rock ''n'' Roll Star - Remastered', 322946),
    ('7HHNKKfD9oNshMTyklBeWu', '3AMHMM2aNG6k3d7ybcQ5bY', 'Shakermaker - Remastered', 308280),
    ('6TlQ5fbojNRuG0hPQMbxeW', '3AMHMM2aNG6k3d7ybcQ5bY', 'Live Forever - Remastered', 276666),
    ('10fOKTNaGzajbX5SdS00Ut', '3AMHMM2aNG6k3d7ybcQ5bY', 'Up In The Sky - Remastered', 268000),
    ('20EkniSUbhZ86v1Oc0hhUI', '3AMHMM2aNG6k3d7ybcQ5bY', 'Columbia - Remastered', 377400),
    ('4jJfa4mO5JjV9Tz2aAxE2M', '3AMHMM2aNG6k3d7ybcQ5bY', 'Supersonic - Remastered', 283786),

    ('1KcFLmFGCfkkAsooF6FJBr', '6tOe4eAF8xNhEkl9WyvsE4', 'Hello - Remastered', 203186),
    ('3czgcbI4YZ4Gg29DOYFAnd', '6tOe4eAF8xNhEkl9WyvsE4', 'Roll With It - Remastered', 240093),
    ('7ygpwy2qP3NbrxVkHvUhXY', '6tOe4eAF8xNhEkl9WyvsE4', 'Wonderwall - Remastered', 258773),
    ('12dU3vAh6AFoJkisorfoUl', '6tOe4eAF8xNhEkl9WyvsE4', 'Don''t Look Back In Anger - Remastered', 289560),
    ('4FDEDR99kqoGZOV88Wpnpg', '6tOe4eAF8xNhEkl9WyvsE4', 'Hey Now! - Remastered', 341906),
    ('1xxPQaR29dAf3yxaafByeD', '6tOe4eAF8xNhEkl9WyvsE4', 'Some Might Say - Remastered', 327920),

    ('4yWTh6qETpN1Vlfg5LBHEV', '4XBCWqCXqCdN72K9SklIjy', 'D''You Know What I Mean? - Remastered', 463066),
    ('2qQBPCjY7WQiqO4LEp9wsU', '4XBCWqCXqCdN72K9SklIjy', 'My Big Mouth - Remastered', 311333),
    ('4B0uIsmygOfQpNSQzanJ5i', '4XBCWqCXqCdN72K9SklIjy', 'Magic Pie - Remastered', 430373),
    ('5WTPhYmwwojFP73O7p3Izi', '4XBCWqCXqCdN72K9SklIjy', 'Stand By Me - Remastered', 355866),
    ('5g4IouEbo38FZi8M1Ga4ey', '4XBCWqCXqCdN72K9SklIjy', 'I Hope, I Think, I Know - Remastered', 262400),
    ('1MkwGdfDRDpDe0y2LkHAFO', '4XBCWqCXqCdN72K9SklIjy', 'The Girl In The Dirty Shirt - Remastered', 349640),

    ('7IuCCezGsyTH9LU4IVL57s', '2EVWJRhbXWsSm7a6jdKv8O', 'The Hindu Times', 226400),
    ('3ujwIruj897uySuJuwoPQN', '2EVWJRhbXWsSm7a6jdKv8O', 'Force Of Nature', 291693),
    ('4RhOMvp9ZPyTuXwLCWvi8O', '2EVWJRhbXWsSm7a6jdKv8O', 'Hung In A Bad Place', 208666),
    ('0SlpFHdk4UHBDzCEoXzy14', '2EVWJRhbXWsSm7a6jdKv8O', 'Stop Crying Your Heart Out', 303120),
    ('2tBxJ43XiwEzhf7Xalc5UG', '2EVWJRhbXWsSm7a6jdKv8O', 'Songbird', 127600),
    ('45AQ5zbSJ0j1nM9daSWeq8', '2EVWJRhbXWsSm7a6jdKv8O', 'Little By Little', 292853);

INSERT INTO images (id, height, width, url) 
VALUES 
    --using negative id values to prevent Entity Framework issues
    --Alive 2007
    (-1, 64, 64, 'https://i.scdn.co/image/ab67616d00004851176ca6e2a6efb0e3aaf4e37a'),
    --Random Access Memories
    (-2, 64, 64, 'https://i.scdn.co/image/ab67616d000048519b9b36b0e22870b9f542d937'),
    --TRON: Legacy
    (-3, 64, 64, 'https://i.scdn.co/image/ab67616d000048518323143296ff7b2801e32789'),
    --Human After All
    (-4, 64, 64, 'https://i.scdn.co/image/ab67616d000048512ed719bad67261c7bf090c70'),
    --Discovery
    (-5, 64, 64, 'https://i.scdn.co/image/ab67616d00004851b33d46dfa2635a47eebf63b2'),
    --Homework
    (-6, 64, 64, 'https://i.scdn.co/image/ab67616d000048518ac778cc7d88779f74d33311'),
    --The Getaway
    (-7, 64, 64, 'https://i.scdn.co/image/ab67616d0000485158406b3f1ac3ceaff7a64fef'),
    --Stadium Arcadium
    (-8, 64, 64, 'https://i.scdn.co/image/ab67616d0000485109fd83d32aee93dceba78517'),
    --By the Way
    (-9, 64, 64, 'https://i.scdn.co/image/ab67616d00004851de1af2785a83cc660155a0c4'),
    --Californication
    (-10, 64, 64, 'https://i.scdn.co/image/ab67616d00004851a9249ebb15ca7a5b75f16a90'),
    --The Balance
    (-11, 64, 64, 'https://i.scdn.co/image/ab67616d00004851835a9e77dae1c928f871ac73'),
    --The Ride
    (-12, 64, 64, 'https://i.scdn.co/image/ab67616d0000485174558885d860bb58d78d1de8'),
    --The Balcony
    (-13, 64, 64, 'https://i.scdn.co/image/ab67616d00004851b87da477ad10e87b09b88d1e'),
    --Definitely Maybe
    (-14, 64, 64, 'https://i.scdn.co/image/ab67616d00004851d86a1e021e7acc7c07c7d668'),
    --(What''s The Story) Morning Glory? 
    (-15, 64, 64, 'https://i.scdn.co/image/ab67616d000048517a4c8c59851c88f6794c3cbf'),
    --Be Here Now
    (-16, 64, 64, 'https://i.scdn.co/image/ab67616d00004851aeda362a434f01d0eff70b4e'),
    --Heathen Chemistry
    (-17, 64, 64, 'https://i.scdn.co/image/ab67616d00004851bd14866d69108524bc32fb37');

INSERT INTO artists_tracks (artist_id, track_id) 
VALUES 
    --Daft Punk
    --Alive 2007
    ('4tZwfgrHOc3mvqYlEYSvVi', '0q9zz6nP5izcUnfYndfVX6'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '0yf4PlZ8rG3MrnqsmTzvMp'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '5pid7wpFMJvXvlYETOIUzo'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '1DtOKrgREeQ9va6f3FAfXf'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '0iYGI01MEFu9ohSNuMVpwL'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '5CYM628A5vNaPlx2hy6Drs'),

    --Random Access Memories
    ('4tZwfgrHOc3mvqYlEYSvVi', '0dEIca2nhcxDUV8C5QkPYb'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '3ctALmweZBapfBdFiIVpji'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '0oks4FnzhNp5QPTZtoet7c'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '7Bxv0WL7UC6WwQpk9TzdMJ'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '2cGxRwrMyEAp8dEbuZaVv6'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '5CMjjywI0eZMixPeqNd75R'),

    --TRON: Legacy
    ('4tZwfgrHOc3mvqYlEYSvVi', '2tRWMCijEFsGPDxBgHxHre'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '3zL0LAsSh3dTO73dSOKWkr'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '6dooKqgWKBVwQLLarxJPDM'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '6NbukMzsdx888nymIiWKlV'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '4csD9dmdLHnarNyu1wG8Iv'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '70RkgofUfQHLl2FT2Mx5zq'),

    --Human After All
    ('4tZwfgrHOc3mvqYlEYSvVi', '3aCKAkMx3yfaj3AO5Gz47e'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '0UZRFYMoz9xmeE2AQUhTDl'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '7LL40F6YdZgeiQ6en1c7Lk'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '60HSQkYSlJVtdRdHgzRsXz'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '4ABWPP59ItFKykdaDF09K5'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '73MAeHX5sqLYfuYclsrvHc'),

    --Discovery
    ('4tZwfgrHOc3mvqYlEYSvVi', '0DiWol3AO6WpXZgp0goxAV'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '3H3cOQ6LBLSvmcaV7QkZEu'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '2VEZx7NWsZ1D0eJ4uv5Fym'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '5W3cjX2J3tjhG8zb6u0qHn'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '6vuPZX9fWESg5js2JFTQRJ'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '63JXZZRbmzooashakb0zbu'),

    --Homework
    ('4tZwfgrHOc3mvqYlEYSvVi', '7fYIXtdOUl8WqXAqQGyIHQ'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '7dTWkvPOPgbGuMk4HDxNpY'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '5pgZpHqfv4TSomtkfGZGrG'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '0MyY4WcN7DIfbSmp5yej5z'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '6nQy5XEEEJKu8FE1FS2Wbt'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '78H72MElkOY9cRnaudxZFY'),

    --Red Hot Chili Peppers
    --The Getaway
    ('0L8ExT028jH3ddEcZwqJJ5', '3bIQIx7hveYPQDdhjZ1kcq'),
    ('0L8ExT028jH3ddEcZwqJJ5', '2oaK4JLVnmRGIO9ytBE1bt'),
    ('0L8ExT028jH3ddEcZwqJJ5', '0cv2LgkvEoQiGgFWcZaAMA'),
    ('0L8ExT028jH3ddEcZwqJJ5', '0pjCkLjbgSLn5c0Ilwuv8z'),
    ('0L8ExT028jH3ddEcZwqJJ5', '2XTkpF9T2PKvcLgamGJGx1'),
    ('0L8ExT028jH3ddEcZwqJJ5', '6GsP3uMCd0Dn5T37C93waZ'),

    --Stadium Arcadium
    ('0L8ExT028jH3ddEcZwqJJ5', '10Nmj3JCNoMeBQ87uw5j8k'),
    ('0L8ExT028jH3ddEcZwqJJ5', '2aibwv5hGXSgw7Yru8IYTO'),
    ('0L8ExT028jH3ddEcZwqJJ5', '3SoDB59Y7dSZLSDBiNJ6o2'),
    ('0L8ExT028jH3ddEcZwqJJ5', '4y84ILALZSa4LyP6H7NVjR'),
    ('0L8ExT028jH3ddEcZwqJJ5', '5f2ZVFERwwh3asebmurZEf'),
    ('0L8ExT028jH3ddEcZwqJJ5', '3gvyksxkLbyKwi0WjCiPXE'),

    --By the Way
    ('0L8ExT028jH3ddEcZwqJJ5', '1f2V8U1BiWaC9aJWmpOARe'),
    ('0L8ExT028jH3ddEcZwqJJ5', '39badcyKTjOtBvv4aywpfs'),
    ('0L8ExT028jH3ddEcZwqJJ5', '42z5BOO8cJda4qWBnHFLQV'),
    ('0L8ExT028jH3ddEcZwqJJ5', '1iFIZUVDBCCkWe705FLXto'),
    ('0L8ExT028jH3ddEcZwqJJ5', '308kjPoIVBzDNExO54sqAi'),
    ('0L8ExT028jH3ddEcZwqJJ5', '1ndGB6rvxKYN9seCYO1dTF'),

    --Californication
    ('0L8ExT028jH3ddEcZwqJJ5', '1Y6DGcTCuMAtw8KB3h4W3q'),
    ('0L8ExT028jH3ddEcZwqJJ5', '3ZwxczCIlt4nA7czQaugvM'),
    ('0L8ExT028jH3ddEcZwqJJ5', '2QOMGq8wVTZbLmh7McrvgF'),
    ('0L8ExT028jH3ddEcZwqJJ5', '3CeYdUfGPCjKMDYyI1PpCh'),
    ('0L8ExT028jH3ddEcZwqJJ5', '3s3oiCHAHLWmKZUYk1ozJG'),
    ('0L8ExT028jH3ddEcZwqJJ5', '34KTEhpPjq6IAgQg2yzJAL'),

    --Catfish and the Bottlemen Tracks
    --The Balance
    ('2xaAOVImG2O6lURwqperlD', '2VcS3oKcOPkubN9LVzZ96l'),
    ('2xaAOVImG2O6lURwqperlD', '27xKntLxqf0HDGVdcNIkcY'),
    ('2xaAOVImG2O6lURwqperlD', '0ZfEeIu7CNHQhbCTiVv3cx'),
    ('2xaAOVImG2O6lURwqperlD', '21slhimb1blAmvpjq0l8rh'),
    ('2xaAOVImG2O6lURwqperlD', '2c8U7Mlax6uIhI9aM4YdpH'),
    ('2xaAOVImG2O6lURwqperlD', '4LdImt7hg85uPpUOPKlLAy'),

    --The Ride
    ('2xaAOVImG2O6lURwqperlD', '5ykbOijJEfRhuo2Td1m0Qd'),
    ('2xaAOVImG2O6lURwqperlD', '2sewj0rFvlr3aEM3bGy12n'),
    ('2xaAOVImG2O6lURwqperlD', '60W7Co2AoP5VVG5Gwu6p5P'),
    ('2xaAOVImG2O6lURwqperlD', '1IMtCtSqlw3VJv3IvCxkaz'),
    ('2xaAOVImG2O6lURwqperlD', '1JblvWxcwHXMLth0c6ssFy'),
    ('2xaAOVImG2O6lURwqperlD', '28VT0090inPlN6bfxoVdmB'),

    --The Balcony
    ('2xaAOVImG2O6lURwqperlD', '7lnXzMcgaK7CnzaQ7wj6k0'),
    ('2xaAOVImG2O6lURwqperlD', '1MHYAqWWdDRePmnqORynrq'),
    ('2xaAOVImG2O6lURwqperlD', '1B241LRKmK6qDDTZfUajmm'),
    ('2xaAOVImG2O6lURwqperlD', '2PAeLLcnw42x5ZszOfFz50'),
    ('2xaAOVImG2O6lURwqperlD', '1rcu88dzWE5GyqtpuWvd0C'),
    ('2xaAOVImG2O6lURwqperlD', '4hEhOvEz9tulJQXZ7hiqkz'),

    --Oasis
    --Definitely Maybe
    ('2DaxqgrOhkeH0fpeiQq2f4', '4bQHPFjRT6O1KdMCd4cD9u'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '7HHNKKfD9oNshMTyklBeWu'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '6TlQ5fbojNRuG0hPQMbxeW'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '10fOKTNaGzajbX5SdS00Ut'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '20EkniSUbhZ86v1Oc0hhUI'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '4jJfa4mO5JjV9Tz2aAxE2M'),

    --(What''s The Story) Morning Glory? 
    ('2DaxqgrOhkeH0fpeiQq2f4', '1KcFLmFGCfkkAsooF6FJBr'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '3czgcbI4YZ4Gg29DOYFAnd'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '7ygpwy2qP3NbrxVkHvUhXY'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '12dU3vAh6AFoJkisorfoUl'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '4FDEDR99kqoGZOV88Wpnpg'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '1xxPQaR29dAf3yxaafByeD'),

    --Be Here Now
    ('2DaxqgrOhkeH0fpeiQq2f4', '4yWTh6qETpN1Vlfg5LBHEV'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '2qQBPCjY7WQiqO4LEp9wsU'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '4B0uIsmygOfQpNSQzanJ5i'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '5WTPhYmwwojFP73O7p3Izi'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '5g4IouEbo38FZi8M1Ga4ey'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '1MkwGdfDRDpDe0y2LkHAFO'),

    --Heathen Chemistry
    ('2DaxqgrOhkeH0fpeiQq2f4', '7IuCCezGsyTH9LU4IVL57s'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '3ujwIruj897uySuJuwoPQN'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '4RhOMvp9ZPyTuXwLCWvi8O'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '0SlpFHdk4UHBDzCEoXzy14'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '2tBxJ43XiwEzhf7Xalc5UG'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '45AQ5zbSJ0j1nM9daSWeq8');

INSERT INTO artists_albums (artist_id, album_id) 
VALUES 
    --Daft Punk
    ('4tZwfgrHOc3mvqYlEYSvVi', '3Bz2QPL8NLBn1d03jXtNkT'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '4m2880jivSbbyEGAKfITCa'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '3AMXFnwHWXCvNr5NCCpLZI'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '2T7DdrOvsqOqU9bGTkjBYu'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '2noRn2Aes5aoNVsU6iWThc'),
    ('4tZwfgrHOc3mvqYlEYSvVi', '5uRdvUR7xCnHmUW8n64n9y'),
    
    --Red Hot Chili Peppers
    ('0L8ExT028jH3ddEcZwqJJ5', '43otFXrY0bgaq5fB3GrZj6'),
    ('0L8ExT028jH3ddEcZwqJJ5', '7xl50xr9NDkd3i2kBbzsNZ'),
    ('0L8ExT028jH3ddEcZwqJJ5', '6deiaArbeoqp1xPEGdEKp1'),
    ('0L8ExT028jH3ddEcZwqJJ5', '0fLhefnjlIV3pGNF9Wo8CD'),

    --Catfish and the Bottlemen Tracks
    ('2xaAOVImG2O6lURwqperlD', '0eELSmJrZpzOKfdO80nJ9r'),
    ('2xaAOVImG2O6lURwqperlD', '07IHAhsG4FnnfHQSb3bbAZ'),
    ('2xaAOVImG2O6lURwqperlD', '0C0OFASoQC57yC12vQhCwN'),

    --Oasis
    ('2DaxqgrOhkeH0fpeiQq2f4', '3AMHMM2aNG6k3d7ybcQ5bY'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '6tOe4eAF8xNhEkl9WyvsE4'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '4XBCWqCXqCdN72K9SklIjy'),
    ('2DaxqgrOhkeH0fpeiQq2f4', '2EVWJRhbXWsSm7a6jdKv8O');

INSERT INTO albums_images (album_id, image_id) 
VALUES 
    --using negative id values to prevent Entity Framework issues
    ('3Bz2QPL8NLBn1d03jXtNkT', -1),
    ('4m2880jivSbbyEGAKfITCa', -2),
    ('3AMXFnwHWXCvNr5NCCpLZI', -3),
    ('2T7DdrOvsqOqU9bGTkjBYu', -4),
    ('2noRn2Aes5aoNVsU6iWThc', -5),
    ('5uRdvUR7xCnHmUW8n64n9y', -6),
    ('43otFXrY0bgaq5fB3GrZj6', -7),
    ('7xl50xr9NDkd3i2kBbzsNZ', -8),
    ('6deiaArbeoqp1xPEGdEKp1', -9),
    ('0fLhefnjlIV3pGNF9Wo8CD', -10),
    ('0eELSmJrZpzOKfdO80nJ9r', -11),
    ('07IHAhsG4FnnfHQSb3bbAZ', -12),
    ('0C0OFASoQC57yC12vQhCwN', -13),
    ('3AMHMM2aNG6k3d7ybcQ5bY', -14),
    ('6tOe4eAF8xNhEkl9WyvsE4', -15),
    ('4XBCWqCXqCdN72K9SklIjy', -16),
    ('2EVWJRhbXWsSm7a6jdKv8O', -17);

INSERT INTO skipped_tracks (id, skipped_date, playlist_id, track_id) 
VALUES 
    --using negative id values to prevent Entity Framework issues
    -- Test Playlist skips
    (-1, '2023-09-10 11:30:30', '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (-2, '2023-09-10 11:30:31', '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (-3, '2023-09-10 11:30:32', '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (-4, '2023-09-10 11:30:33', '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (-5, '2023-09-10 11:30:34', '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),
    (-6, '2023-09-10 11:30:35', '3RWXoorVz13dot7I11eiu6', '0oks4FnzhNp5QPTZtoet7c'),

    (-7, '2023-09-10 11:31:30', '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (-8, '2023-09-10 11:31:31', '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (-9, '2023-09-10 11:31:32', '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (-10, '2023-09-10 11:31:33', '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),
    (-11, '2023-09-10 11:31:34', '3RWXoorVz13dot7I11eiu6', '0yf4PlZ8rG3MrnqsmTzvMp'),

    (-12, '2023-09-10 11:32:30', '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),
    (-13, '2023-09-10 11:32:31', '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),
    (-14, '2023-09-10 11:32:32', '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),
    (-15, '2023-09-10 11:32:33', '3RWXoorVz13dot7I11eiu6', '4csD9dmdLHnarNyu1wG8Iv'),

    (-16, '2023-09-10 11:33:30', '3RWXoorVz13dot7I11eiu6', '73MAeHX5sqLYfuYclsrvHc'),
    (-17, '2023-09-10 11:33:31', '3RWXoorVz13dot7I11eiu6', '73MAeHX5sqLYfuYclsrvHc'),
    (-18, '2023-09-10 11:33:32', '3RWXoorVz13dot7I11eiu6', '73MAeHX5sqLYfuYclsrvHc'),

    (-19, '2023-09-10 11:34:31', '3RWXoorVz13dot7I11eiu6', '63JXZZRbmzooashakb0zbu'),
    (-20, '2023-09-10 11:34:32', '3RWXoorVz13dot7I11eiu6', '63JXZZRbmzooashakb0zbu'),

    (-21, '2023-09-10 11:35:30', '3RWXoorVz13dot7I11eiu6', '78H72MElkOY9cRnaudxZFY'),

    -- Test Playlist 2 skips
    (-22, '2023-09-10 11:36:30', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (-23, '2023-09-10 11:36:31', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (-24, '2023-09-10 11:36:32', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (-25, '2023-09-10 11:36:33', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (-26, '2023-09-10 11:36:34', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (-27, '2023-09-10 11:36:35', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),
    (-28, '2023-09-10 11:36:36', '4jkoiv8phuKuhXboalicqu', '0cv2LgkvEoQiGgFWcZaAMA'),

    (-29, '2023-09-10 11:37:30', '4jkoiv8phuKuhXboalicqu', '5f2ZVFERwwh3asebmurZEf'),
    (-30, '2023-09-10 11:37:31', '4jkoiv8phuKuhXboalicqu', '5f2ZVFERwwh3asebmurZEf'),
    (-31, '2023-09-10 11:37:32', '4jkoiv8phuKuhXboalicqu', '5f2ZVFERwwh3asebmurZEf'),

    (-32, '2023-09-10 11:38:30', '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (-33, '2023-09-10 11:38:31', '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (-34, '2023-09-10 11:38:32', '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (-35, '2023-09-10 11:38:33', '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),
    (-36, '2023-09-10 11:38:34', '4jkoiv8phuKuhXboalicqu', '42z5BOO8cJda4qWBnHFLQV'),

    (-37, '2023-09-10 11:39:30', '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (-38, '2023-09-10 11:39:31', '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (-39, '2023-09-10 11:39:32', '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (-40, '2023-09-10 11:39:33', '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (-41, '2023-09-10 11:39:34', '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),
    (-42, '2023-09-10 11:39:35', '4jkoiv8phuKuhXboalicqu', '3s3oiCHAHLWmKZUYk1ozJG'),

    -- Test Playlist 3 skips
    (-43, '2023-09-10 11:40:30', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-44, '2023-09-10 11:40:31', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-45, '2023-09-10 11:40:32', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-46, '2023-09-10 11:40:33', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-47, '2023-09-10 11:40:34', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-48, '2023-09-10 11:40:35', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-49, '2023-09-10 11:40:36', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),
    (-50, '2023-09-10 11:40:37', '1aQvavrk6m4hhEj1NJ0rQB', '21slhimb1blAmvpjq0l8rh'),

    (-51, '2023-09-10 11:41:30', '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (-52, '2023-09-10 11:41:31', '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (-53, '2023-09-10 11:41:32', '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (-54, '2023-09-10 11:41:33', '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),
    (-55, '2023-09-10 11:41:34', '1aQvavrk6m4hhEj1NJ0rQB', '28VT0090inPlN6bfxoVdmB'),

    (-56, '2023-09-10 11:42:30', '1aQvavrk6m4hhEj1NJ0rQB', '4hEhOvEz9tulJQXZ7hiqkz'),
    (-57, '2023-09-10 11:42:31', '1aQvavrk6m4hhEj1NJ0rQB', '4hEhOvEz9tulJQXZ7hiqkz'),
    (-58, '2023-09-10 11:42:32', '1aQvavrk6m4hhEj1NJ0rQB', '4hEhOvEz9tulJQXZ7hiqkz'),

    (-59, '2023-09-10 11:43:30', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-60, '2023-09-10 11:43:31', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-61, '2023-09-10 11:43:32', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-62, '2023-09-10 11:43:33', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-63, '2023-09-10 11:43:34', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-64, '2023-09-10 11:43:35', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-65, '2023-09-10 11:43:36', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-66, '2023-09-10 11:43:37', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),
    (-67, '2023-09-10 11:43:38', '1aQvavrk6m4hhEj1NJ0rQB', '0ZfEeIu7CNHQhbCTiVv3cx'),

    (-68, '2023-09-10 11:44:30', '1aQvavrk6m4hhEj1NJ0rQB', '2PAeLLcnw42x5ZszOfFz50'),
    (-69, '2023-09-10 11:44:31', '1aQvavrk6m4hhEj1NJ0rQB', '2PAeLLcnw42x5ZszOfFz50'),

    (-70, '2023-09-10 11:45:30', '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),
    (-71, '2023-09-10 11:45:31', '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),
    (-72, '2023-09-10 11:45:32', '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),
    (-73, '2023-09-10 11:45:33', '1aQvavrk6m4hhEj1NJ0rQB', '1JblvWxcwHXMLth0c6ssFy'),

    -- Test Playlist 3 skips
    (-74, '2023-09-10 11:46:30', '4btLcc6A1UmNR7sa2dqlLJ', '7HHNKKfD9oNshMTyklBeWu'),
    (-75, '2023-09-10 11:46:31', '4btLcc6A1UmNR7sa2dqlLJ', '7HHNKKfD9oNshMTyklBeWu'),
    (-76, '2023-09-10 11:46:32', '4btLcc6A1UmNR7sa2dqlLJ', '7HHNKKfD9oNshMTyklBeWu'),

    (-77, '2023-09-10 11:47:30', '4btLcc6A1UmNR7sa2dqlLJ', '20EkniSUbhZ86v1Oc0hhUI'),
    (-78, '2023-09-10 11:47:31', '4btLcc6A1UmNR7sa2dqlLJ', '20EkniSUbhZ86v1Oc0hhUI'),

    (-79, '2023-09-10 11:48:30', '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (-80, '2023-09-10 11:48:31', '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (-81, '2023-09-10 11:48:32', '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (-82, '2023-09-10 11:48:33', '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (-83, '2023-09-10 11:48:34', '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),
    (-84, '2023-09-10 11:48:35', '4btLcc6A1UmNR7sa2dqlLJ', '4FDEDR99kqoGZOV88Wpnpg'),

    (-85, '2023-09-10 11:49:30', '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),
    (-86, '2023-09-10 11:49:31', '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),
    (-87, '2023-09-10 11:49:32', '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),
    (-88, '2023-09-10 11:49:33', '4btLcc6A1UmNR7sa2dqlLJ', '4yWTh6qETpN1Vlfg5LBHEV'),

    (-89, '2023-09-10 11:50:30', '4btLcc6A1UmNR7sa2dqlLJ', '5g4IouEbo38FZi8M1Ga4ey'),

    (-90, '2023-09-10 11:51:30', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (-91, '2023-09-10 11:51:31', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (-92, '2023-09-10 11:51:32', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (-93, '2023-09-10 11:51:33', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (-94, '2023-09-10 11:51:34', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (-95, '2023-09-10 11:51:35', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),
    (-96, '2023-09-10 11:51:36', '4btLcc6A1UmNR7sa2dqlLJ', '3ujwIruj897uySuJuwoPQN'),

    (-97, '2023-09-10 11:52:30', '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG'),
    (-98, '2023-09-10 11:52:31', '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG'),
    (-99, '2023-09-10 11:52:32', '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG'),
    (-100, '2023-09-10 11:52:33', '4btLcc6A1UmNR7sa2dqlLJ', '2tBxJ43XiwEzhf7Xalc5UG');