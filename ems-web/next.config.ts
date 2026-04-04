import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* Enables Docker image using standalone output (see ems-web/Dockerfile). */
  output: "standalone",
};

export default nextConfig;
