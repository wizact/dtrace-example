#!/bin/bash
set -e


folders=(
    "init"
    "tables"
)

for folder in ${folders[@]}
do

    echo "Processing all files matching $folder"
    
    
    for file in $(ls $folder/*.sql)
    do
        echo "Processing $file file"
        psql -p 5432 -h localhost -U postgres -d postgres < $file
    done
done



seed_folder="data"

echo "Processing seed data in "

for file in $(ls $seed_folder/*.sql)
do
    echo "Processing $file file"
    psql -p 5432 -h localhost -U postgres -d postgres < $file
done

echo "Processing COMPLETE"