/*
Navicat MySQL Data Transfer

Source Server         : 20150518
Source Server Version : 50173
Source Host           : 10.22.102.19:3306
Source Database       : intensiveuse

Target Server Type    : MYSQL
Target Server Version : 50173
File Encoding         : 65001

Date: 2015-06-03 15:14:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for agricultureland
-- ----------------------------
DROP TABLE IF EXISTS `agricultureland`;
CREATE TABLE `agricultureland` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Subtotal` float(10,2) DEFAULT NULL,
  `Arable` float(10,2) DEFAULT NULL,
  `Garden` float(10,2) DEFAULT NULL,
  `Forest` float(10,2) DEFAULT NULL,
  `Meadow` float(10,2) DEFAULT NULL,
  `Other` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for constructionland
-- ----------------------------
DROP TABLE IF EXISTS `constructionland`;
CREATE TABLE `constructionland` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `SubTotal` float(10,2) DEFAULT NULL,
  `Town` float(10,2) DEFAULT NULL,
  `MiningLease` float(10,2) DEFAULT NULL,
  `County` float(10,2) DEFAULT NULL,
  `Traffic` float(10,2) DEFAULT NULL,
  `OtherConstruction` float(10,2) DEFAULT NULL,
  `Other` float(10,2) DEFAULT NULL,
  `Sum` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for economy
-- ----------------------------
DROP TABLE IF EXISTS `economy`;
CREATE TABLE `economy` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Current` float(10,2) DEFAULT NULL,
  `Compare` float(10,2) DEFAULT NULL,
  `Aggregate` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for exponent
-- ----------------------------
DROP TABLE IF EXISTS `exponent`;
CREATE TABLE `exponent` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PUII1` float(10,2) DEFAULT NULL,
  `EUII1` float(10,2) DEFAULT NULL,
  `EUII2` float(10,2) DEFAULT NULL,
  `PGCI` float(10,2) DEFAULT NULL,
  `EGCI1` float(10,2) DEFAULT NULL,
  `EGCI2` float(10,2) DEFAULT NULL,
  `EGCI3` float(10,2) DEFAULT NULL,
  `PEI1` float(10,2) DEFAULT NULL,
  `EEI` float(10,2) DEFAULT NULL,
  `ULAPI1` float(10,2) DEFAULT NULL,
  `ULAPI2` float(10,2) DEFAULT NULL,
  `Type` int(11) NOT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for files
-- ----------------------------
DROP TABLE IF EXISTS `files`;
CREATE TABLE `files` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FileName` varchar(127) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `SavePath` varchar(127) DEFAULT NULL,
  `State` int(1) NOT NULL,
  `ProcessMessage` varchar(1023) DEFAULT NULL,
  `FileTypeName` varchar(255) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for foundation
-- ----------------------------
DROP TABLE IF EXISTS `foundation`;
CREATE TABLE `foundation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PUII` varchar(1023) DEFAULT NULL,
  `EUII1` varchar(1023) DEFAULT NULL,
  `EUII2` varchar(1023) DEFAULT NULL,
  `PGCI1` varchar(1023) DEFAULT NULL,
  `EGCI1` varchar(1023) DEFAULT NULL,
  `EGCI2` varchar(1023) DEFAULT NULL,
  `EGCI3` varchar(1023) DEFAULT NULL,
  `PEII` varchar(1023) DEFAULT NULL,
  `EEI` varchar(1023) DEFAULT NULL,
  `ULAPI1` varchar(1023) DEFAULT NULL,
  `ULAPI2` varchar(1023) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for indexweight
-- ----------------------------
DROP TABLE IF EXISTS `indexweight`;
CREATE TABLE `indexweight` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UII` float(10,2) DEFAULT NULL,
  `GCI` float(10,2) DEFAULT NULL,
  `EI` float(10,2) DEFAULT NULL,
  `API` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for landsupply
-- ----------------------------
DROP TABLE IF EXISTS `landsupply`;
CREATE TABLE `landsupply` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Sum` float(10,2) DEFAULT NULL,
  `Append` float(10,2) DEFAULT NULL,
  `Stock` float(10,2) DEFAULT NULL,
  `UnExploit` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for newconstruction
-- ----------------------------
DROP TABLE IF EXISTS `newconstruction`;
CREATE TABLE `newconstruction` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Construction` float(10,2) DEFAULT NULL,
  `Town` float(10,2) DEFAULT NULL,
  `CID` int(11) NOT NULL,
  `Year` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for people
-- ----------------------------
DROP TABLE IF EXISTS `people`;
CREATE TABLE `people` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PermanentSum` float(10,2) DEFAULT NULL,
  `Town` float(10,2) DEFAULT NULL,
  `County` float(10,2) DEFAULT NULL,
  `HouseHold` float(10,2) DEFAULT NULL,
  `Agriculture` float(10,2) DEFAULT NULL,
  `NonFram` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for ratify
-- ----------------------------
DROP TABLE IF EXISTS `ratify`;
CREATE TABLE `ratify` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Area` float(10,2) DEFAULT NULL,
  `Already` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for region
-- ----------------------------
DROP TABLE IF EXISTS `region`;
CREATE TABLE `region` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(1023) NOT NULL,
  `Code` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for statistic
-- ----------------------------
DROP TABLE IF EXISTS `statistic`;
CREATE TABLE `statistic` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Year` int(11) NOT NULL,
  `People` bit(1) NOT NULL,
  `Economy` bit(1) NOT NULL,
  `Agriculture` bit(1) NOT NULL,
  `ConstructionLand` bit(1) NOT NULL,
  `NewConstruction` bit(1) NOT NULL,
  `LandSupply` bit(1) NOT NULL,
  `Ratify` bit(1) NOT NULL,
  `Ideal` bit(1) NOT NULL,
  `IndexWeight` bit(1) NOT NULL,
  `SubIndex` bit(1) NOT NULL,
  `Weight` bit(1) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for subindex
-- ----------------------------
DROP TABLE IF EXISTS `subindex`;
CREATE TABLE `subindex` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `PUII` float(10,2) DEFAULT NULL,
  `EUII` float(10,2) DEFAULT NULL,
  `PGCI` float(10,2) DEFAULT NULL,
  `EGCI` float(10,2) DEFAULT NULL,
  `PEI` float(10,2) DEFAULT NULL,
  `EEI` float(10,2) DEFAULT NULL,
  `ULAPI` float(10,2) DEFAULT NULL,
  `Year` int(11) NOT NULL,
  `RID` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
