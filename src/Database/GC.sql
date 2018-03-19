/*
SQLyog Ultimate v12.09 (32 bit)
MySQL - 5.0.51b-community-nt-log : Database - GC
*********************************************************************
*/


/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`GC` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `GC`;

/*Table structure for table `contas` */

DROP TABLE IF EXISTS `contas`;

CREATE TABLE `contas` (
  `userid` int(4) NOT NULL auto_increment,
  `usuario` varchar(32) default NULL,
  `senha` varchar(32) default NULL,
  `online` int(1) default '0',
  `ban` int(1) NOT NULL default '0',
  `moderador` int(1) NOT NULL default '0',
  `tamanhodoinventario` int(11) default '0',
  PRIMARY KEY  (`userid`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Data for the table `contas` */

insert  into `contas`(`userid`,`usuario`,`senha`,`online`,`ban`,`moderador`,`tamanhodoinventario`) values (1,'gm1','202cb962ac59075b964b07152d234b70',0,0,0,500);

/*Table structure for table `equipamentos` */

DROP TABLE IF EXISTS `equipamentos`;

CREATE TABLE `equipamentos` (
  `userid` int(11) default NULL,
  `itemorderNO` int(11) default NULL,
  `itemid` int(11) default NULL,
  `itemuid` int(11) default NULL,
  `itemtipo` int(11) default NULL,
  `personagemid` int(11) default NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `equipamentos` */

/*Table structure for table `gamepoints` */

DROP TABLE IF EXISTS `gamepoints`;

CREATE TABLE `gamepoints` (
  `userid` int(11) default NULL,
  `GP` int(11) default NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `gamepoints` */

insert  into `gamepoints`(`userid`,`GP`) values (1,1000);

/*Table structure for table `inventario` */

DROP TABLE IF EXISTS `inventario`;

CREATE TABLE `inventario` (
  `userid` int(11) default NULL,
  `itemuid` int(11) default NULL,
  `itemid` int(11) default NULL,
  `itemtipo` int(11) default NULL,
  `quantidade` int(11) default NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `inventario` */

/*Table structure for table `nicknames` */

DROP TABLE IF EXISTS `nicknames`;

CREATE TABLE `nicknames` (
  `userid` int(11) default NULL,
  `nickname` varchar(12) default NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `nicknames` */

insert  into `nicknames`(`userid`,`nickname`) values (1,'Player');

/*Table structure for table `personagems` */

DROP TABLE IF EXISTS `personagems`;

CREATE TABLE `personagems` (
  `userid` int(11) default NULL,
  `personagemid` int(11) default NULL,
  `classe` int(11) default '0',
  `experiencia` int(11) default '100',
  `nivel` int(11) default '1',
  `mascote` int(11) default '0',
  `vitoria` int(11) default '0',
  `derrota` int(11) default '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `personagems` */

insert  into `personagems`(`userid`,`personagemid`,`classe`,`experiencia`,`nivel`,`mascote`,`vitoria`,`derrota`) values (1,0,0,100,1,0,0,0),(1,1,0,100,1,0,0,0),(1,13,0,0,0,0,100,20);

/*Table structure for table `servidores` */

DROP TABLE IF EXISTS `servidores`;

CREATE TABLE `servidores` (
  `id` int(4) NOT NULL auto_increment,
  `name` varchar(255) default NULL,
  `descricao` varbinary(255) default NULL,
  `IP` varchar(255) default NULL,
  `PORTA` int(4) default NULL,
  `UsuariosOnline` int(4) default NULL,
  `MaximoDePlayers` int(4) default NULL,
  `Flag` int(4) default NULL,
  `Tipo` int(4) default NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

/*Data for the table `servidores` */

insert  into `servidores`(`id`,`name`,`descricao`,`IP`,`PORTA`,`UsuariosOnline`,`MaximoDePlayers`,`Flag`,`Tipo`) values (1,'missão','Servidor de Missao','127.0.0.1',9400,0,50,323,0),(2,'testes','bla bla bla','127.0.0.1',9400,0,50,323,0);

/*Table structure for table `spleft` */

DROP TABLE IF EXISTS `spleft`;

CREATE TABLE `spleft` (
  `userid` int(11) default NULL,
  `personagemid` int(11) default NULL,
  `SP` int(11) default NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `spleft` */

insert  into `spleft`(`userid`,`personagemid`,`SP`) values (1,8,70);

/*Table structure for table `vidabonus` */

DROP TABLE IF EXISTS `vidabonus`;

CREATE TABLE `vidabonus` (
  `userid` int(11) default NULL,
  `quantidade` int(11) default '10'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Data for the table `vidabonus` */

insert  into `vidabonus`(`userid`,`quantidade`) values (1,10);

/* Procedure structure for procedure `GC_Register` */

/*!50003 DROP PROCEDURE IF EXISTS  `GC_Register` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `GC_Register`(in GCLogin VARCHAR(20),
	in GCSenha VARCHAR(55),
    in GCNick  VARCHAR(20),
    in GCpb    INT(11))
BEGIN
	declare GCid int(11);
	INSERT INTO `account`(login,Passwd,Nick,Gamepoint)
    VALUES (GCLogin,GCSenha,GCNick,GCpb); -- Contas
	SET GCid = LAST_INSERT_ID();
    
    INSERT INTO `attendancepoints`(LoginUID,points)
    VALUES(GCid, 520); -- Calendario
    
    INSERT INTO `character`(LoginUID)
    VALUES(GCid);
    
    INSERT INTO `charslot`(LoginUID,Slots)
    VALUES(GCid, 1);
    
    INSERT INTO `tutorialmode`(id,active)
    VALUES(GCid, 0);
    
    INSERT INTO `userauthlevel`(id,authlevel)
    VALUES(GCid, 0);
    
    INSERT INTO `currentcashvirtual`(LoginUID,Login,Cash)
    VALUES(GCid, GCLogin, 0);
END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
