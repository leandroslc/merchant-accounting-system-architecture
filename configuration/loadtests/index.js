import http from 'k6/http';
import { check } from 'k6';

const LoginEndpoint = "http://simple-auth-api:8080/v1/tokens";
const BalanceEndpoint = "http://api-gateway:8000/balance?day=2024-05-10";

export const options = {
  thresholds: {
    http_req_failed: ['rate<0.05'],
  },
  scenarios: {
    average_load: {
      executor: 'ramping-vus',
      stages: [
        { duration: '10s', target: 50 },
      ],
    },
  },
};

function request(token) {
  return http.get(BalanceEndpoint, { headers: { Authorization: `Bearer ${token}` } });
}

export function setup() {
  const res = http.get(LoginEndpoint);

  return res.body;
}

export default function(token) {
  const res = request(token);

  check(res, {
    'response code was 200': (res) => res.status == 200,
  });
}
