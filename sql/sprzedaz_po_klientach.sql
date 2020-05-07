SELECT 
	k.kh_Symbol
	,SUM(d.ob_WartBrutto) AS WartoscBrutto
	,SUM(d.ob_WartNetto) AS WartoscNetto
FROM Test.dbo.vwDokumenty d
JOIN Test.dbo.kh__Kontrahent k ON k.kh_Id = d.dok_OdbiorcaId
WHERE CHARINDEX('FS', d.dok_nrpelny) > 0
AND d.dok_DataWyst BETWEEN '20200301' AND '20200331'
GROUP BY k.kh_Symbol

