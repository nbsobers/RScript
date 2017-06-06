args <- commandArgs(trailingOnly = FALSE)

library(RODBCext)
library(RODBC)



f <- args[2]
e <-args[3]
d<- args[4]
t <- args[5]
cf<-args[6]


print(f)
print(e)
print(d)
print(t)
print(cf)






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
