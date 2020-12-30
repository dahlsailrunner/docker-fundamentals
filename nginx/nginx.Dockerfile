FROM nginx

COPY nginx/nginx.local.conf /etc/nginx/nginx.conf
COPY nginx/id-local.crt /etc/ssl/certs/id-local.globomantics.com.crt
COPY nginx/id-local.key /etc/ssl/private/id-local.globomantics.com.key
COPY nginx/www-local.crt /etc/ssl/certs/www-local.globomantics.com.crt
COPY nginx/www-local.key /etc/ssl/private/www-local.globomantics.com.key