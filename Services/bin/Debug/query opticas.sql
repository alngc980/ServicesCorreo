USE [Sis_optica]
GO
/****** Object:  StoredProcedure [dbo].[rpt_Informe]    Script Date: 16/12/2021 14:00:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER proc [dbo].[rpt_Informe](
@var date
)as 
begin
	DECLARE @nIdEmpresa VARCHAR(20) = '16'

	CREATE TABLE #Resultados(Script VARCHAR(MAX))

	select	ROW_NUMBER() OVER(ORDER BY V.ven_id) AS fila,
			@nIdEmpresa empresa,
			v.ven_id id, 
			V.td_id tipoDoc,
			v.ven_serie,
			v.ven_correlativo,
			v.ven_fecha,
			C.cli_numero documento,
			C.cli_razon nombre,
			v.ven_total v_total,
			v.ven_ley,
			vD.vd_cant,
			1 Producto,
			vd.pro_id codpro,
			'NIU' UM,
			pt.pro_descripcion nombre_pro,
			vd.vd_precio,
			vd.vd_subtotal,
			ISNULL(NC.nc_id,0) Extorno,
			'E000' SerieExtorno,
			'00000' CorrelaExtorno,
			'Motivo' Motivo
		INTO #Temp
	from venta v
	INNER JOIN clientes C ON C.cli_id = V.cli_id
	INNER JOIN venta_detalle VD ON VD.ven_id = V.ven_id
	inner join productos pt on pt.pro_id = vd.pro_id
	LEFT JOIN Nota_credito NC ON LEFT(NC.nc_serie,4)=V.ven_serie AND RIGHT(NC.nc_serie,7)=V.ven_correlativo
	where CONVERT(date, v.ven_fecha) = CONVERT(date, @var)
	order by ROW_NUMBER() OVER(ORDER BY V.ven_id),V.td_id,v.ven_fecha


	INSERT INTO #Resultados
	select char(13)+CHAR(10) + '--FECHA  :   ' + convert(varchar(10),@var) Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) + 'USE DBFacturador' Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) + 'GO' Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) + 'CREATE TABLE #VentasFact(	nIdEmpresa int NULL,	nIdVenta int NULL,	nTipoDoc int NULL,	cSerie varchar(10) NULL,	cCorrelativo varchar(10) NULL,	dFechaHora datetime NULL,	cClienteDoc varchar(18) NULL,	cClienteRazonSocial varchar(100) NULL,	dMontoTotal Decimal(18, 2) NULL,	cLeyenda varchar(max) NULL,	dCant Decimal(18, 2) NULL,	nTipoBienPS int NULL,	CodItemProd varchar(100) NULL,	cUm varchar(10) NULL,	cDescripcion varchar(250) NULL,	dPrecioUnit Decimal(18, 2) NULL, dIGV Decimal(18, 2) NULL,dSubtotal decimal(18, 2) NULL,	bAnulado int NULL,	cSerieAnulado varchar(10) NULL,	cCorrelativoAnulado varchar(10) NULL,	cMotivoAnulado varchar(50) NULL)' Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) Script 
	
	declare @i int = 1
	declare @t int = (select COUNT(*) from #Temp)
	declare @asc xml = ''

	while @i <= @t
	begin
		set @asc = ''
		
		INSERT INTO #Resultados
		select CHAR(10)+
		'INSERT INTO #VentasFact VALUES('	+ CAST(@nIdEmpresa as varchar) + ','
											+ CAST(t.id as varchar) + ','
											+ CAST(tipoDoc as varchar) + ','
											+ '''' + CAST(t.ven_serie as varchar) + ''','
											+ '''' + CAST(t.ven_correlativo as varchar) + ''','
											+ '''' + replace(CONVERT(varchar,t.ven_fecha,20),'-','') + ''','
											+ '''' + CAST(t.documento as varchar) +''','
											+ '''' + CAST(t.nombre as varchar) + ''','
											+ CAST(t.v_total as varchar) + ','
											+ 'dbo.fn_ConvertirNumeroLetra('+ CAST(t.v_total as varchar) + ','''+ 'SOLES' +''')'+ ','
											+ CAST(t.vd_cant as varchar) + ','
											+ CAST(1 as varchar) + ','
											+ '''' + CAST(codpro as varchar) + ''','
											+ '''' + CAST('NIU' as varchar) + ''','
											+ '''' + CAST(t.nombre_pro as varchar) + ''','
											+ CAST(t.vd_precio as varchar) + ','--IMPONIBLE
											+ CAST(0 as varchar) + ','--IGV
											+ CAST(t.vd_subtotal as varchar) + ','
											+ CAST(t.Extorno as varchar) + ','
											+ '''' + CAST(t.SerieExtorno as varchar) + ''','
											+ '''' + CAST(t.CorrelaExtorno as varchar) + ''','
											+ '''' + CAST(t.Motivo as varchar) + '''' +
											')'
		from #Temp t where fila = @i
		--FOR XML PATH ('') 
					
		set @i = @i + 1
	end

	INSERT INTO #Resultados
	select char(13)+CHAR(10)																												Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) + 'SELECT CAST(count(*)  AS VARCHAR) + ''' + ' COMPROBANTES' + ''' [RESULTADOS] FROM #VentasFact f where f.nIdVenta not in ( select distinct nIdVenta from DBFacturador..VentasFact where nIdEmpresa = '+ @nIdEmpresa +') UNION' UNION
	select char(13)+CHAR(10) + 'SELECT CAST(count(*)  AS VARCHAR) + ''' + ' BOLETAS' + ''' [RESULTADOS] FROM #VentasFact f where nTipoDoc = 1 AND f.nIdVenta not in ( select distinct nIdVenta from DBFacturador..VentasFact where nIdEmpresa = '+ @nIdEmpresa +') UNION'UNION
	select char(13)+CHAR(10) + 'SELECT CAST(count(*)  AS VARCHAR) + ''' + ' FACTURAS' + ''' [RESULTADOS] FROM #VentasFact f where nTipoDoc = 2 AND f.nIdVenta not in ( select distinct nIdVenta from DBFacturador..VentasFact where nIdEmpresa = '+ @nIdEmpresa +')'

	INSERT INTO #Resultados
	select char(13)+CHAR(10)																												Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) +	'INSERT INTO DBFacturador..VentasFact(nIdEmpresa,nIdVenta,nTipoDoc,cSerie,cCorrelativo,dFechaHora,cClienteDoc,cClienteRazonSocial,dMontoTotal,cLeyenda,dCant,nTipoBienPS,cCodProductoServi,cUm,cDescripcion,dPrecioUnit,dIGV,dSubtotal,bAnulado,cSerieAnulado,cCorrelativoAnulado,cMotivoAnulado)'Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) +	'SELECT * FROM #VentasFact f'																				Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) +	'where f.nIdVenta not in ( select distinct nIdVenta from DBFacturador..VentasFact where nIdEmpresa = ' + @nIdEmpresa +')'	Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10)																												Script
	INSERT INTO #Resultados
	select char(13)+CHAR(10) +	'DROP TABLE #VentasFact'

	SELECT * FROM #Resultados

	DROP TABLE #Resultados
	DROP TABLE #Temp
end