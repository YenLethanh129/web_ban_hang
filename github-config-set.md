
```bash

gh secret set DOCKER_HUB_USERNAME --body "your-dockerhub-username"

gh secret set DOCKER_HUB_TOKEN --body "your-dockerhub-token"

gh secret set PROD_HOST --body "your-production-server-ip"

gh secret set PROD_USERNAME --body "your-server-username"

gh secret set PROD_SSH_KEY --body "$(cat ~/.ssh/id_rsa)"
```
