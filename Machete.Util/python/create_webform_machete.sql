delimiter $$

CREATE TABLE `webform_machete` (
  `sid` int(11) NOT NULL,
  `success` tinyint(1) NOT NULL DEFAULT '0',
  `tries` int(11) DEFAULT NULL,
  `last_attempt` datetime DEFAULT NULL,
  PRIMARY KEY (`sid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1$$