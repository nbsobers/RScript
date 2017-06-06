args <- commandArgs(trailingOnly = TRUE)

library(RODBCext)
library(RODBC)


f<- args[1]
e <-args[2]
d<- args[3]
t <- args[4]
cf<-args[5]







s <- "SELECT  *
  FROM [AdventureWorks2016CTP3].[Purchasing].[PurchaseOrderDetail]

  where purchaseorderID = ?   and     StockedQty <= ? "



ConnectionString <- paste('driver={SQL Server};server=',f,';database=',e,';uid=',d,'; pwd=',t,';trusted_connection=no',sep="")


conn <- odbcDriverConnect(ConnectionString) 


cf2 <- read.csv(paste(cf), stringsAsFactors = F)





cf3<- data.frame(cf2 )





cf3





data <- data.frame(sqlExecute (  conn, paste(s),data=cf3,   fetch=TRUE))

data 

write.csv(data,file="D:/Dev/POC/RScript/RScript/Output/Generated.csv")


close(conn)
