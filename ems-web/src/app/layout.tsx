import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import { AppNav } from "@/shared/components/AppNav";
import { OrganizationGate } from "@/providers/OrganizationGate";
import { OrganizationProvider } from "@/providers/OrganizationProvider";
import { ReactQueryProvider } from "@/providers/ReactQueryProvider";
import "./globals.css";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "EMS",
  description: "Employee Management System web client",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html
      lang="en"
      className={`${geistSans.variable} ${geistMono.variable} h-full antialiased`}
    >
      <body className="flex min-h-full flex-col">
        <ReactQueryProvider>
          <OrganizationProvider>
            <OrganizationGate>
              <AppNav />
              {children}
            </OrganizationGate>
          </OrganizationProvider>
        </ReactQueryProvider>
      </body>
    </html>
  );
}
