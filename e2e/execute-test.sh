#!/bin/bash

docker compose down -v
docker compose up -d
rm -r allure-results
for i in $(seq 1 10); do
    sleep 2;
    healty=$(docker inspect --format='{{.State.Health.Status}}' mysql)
    if [ $healty = "healthy" ]; then
        break;
    fi
done
npx playwright test --reporter=line,html,allure-playwright
